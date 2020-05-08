using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using R5T.Bath;
using R5T.Cambridge.Types;
using R5T.Chalandri;
using R5T.Evosmos;
using R5T.Ilioupoli.Default;
using R5T.Lombardy;
using R5T.Magyar.IO;
using R5T.Odense;
using R5T.Liverpool;
using R5T.Solgene;
using R5T.Soltana;
using R5T.Solutas;

using InMemoryVisualStudioSolutionFileOperator = R5T.Soltana.IVisualStudioSolutionFileOperator;


namespace R5T.Solitude.Standard.Construction
{
    class Program
    {
        #region Static

        private static IEnumerable<(string relativeProjectFilePath, string projectGuidString)> GetAllProjectsInformation()
        {
            yield return (@"R5T.Canterbury.Standard.Construction\R5T.Canterbury.Standard.Construction.csproj", "4094A103-2F9F-47CB-822C-9823469D04C3");
            yield return (@"R5T.Canterbury.Standard\R5T.Canterbury.Standard.csproj", "AD5DB089-24BC-4E13-8400-7C6CD706FFF1");
            yield return (@"R5T.Canterbury.Standard.Testing\R5T.Canterbury.Standard.Testing.csproj", "042A8C51-4EF7-4044-B271-942E369871C5");
            yield return (@"..\..\R5T.Canterbury.Default\source\R5T.Canterbury.Default\R5T.Canterbury.Default.csproj", "422FED08-9C3D-4988-971E-BFC0293142FE");
            yield return (@"..\..\R5T.Canterbury.Base\source\R5T.Canterbury.Base\R5T.Canterbury.Base.csproj", "DA093E67-20EB-4E4C-9815-A09166E07DBF");
            yield return (@"..\..\R5T.Dacia\source\R5T.Dacia\R5T.Dacia.csproj", "DE097B06-7297-4E7C-B38D-F47CD54316C3");
        }

        #endregion

        public void Run()
        {
            //this.CreateNewSolutionFile();
            //this.CreateNewSolutionFileAddFirstProject();
            //this.CreateNewSolutionFileAddAllProjects();
            //this.RemoveAllProjects();
            //this.AddMultipleSolutionFolders();
            //this.ListRootSolutionFolders_Example();
            //this.ListProjectsInSolutionFolder();
            //this.MoveProjectsOutOfSolutionFolder();
            //this.RemoveSolutionFolderAndContents();
            //this.ListRootProjectFilePaths();
            //this.ListProjectsInSolutionFile();
        }

        private void CreateNewSolutionFile()
        {
            this.CreateNewSolutionFileAndReturnPath();
        }

        private string CreateNewSolutionFileAndReturnPath()
        {
            var solutionFilePath = this.GetTemporaryDirectorySolutionFilePath();

            this.VisualStudioSolutionFileOperator.CreateNewSolutionFile(solutionFilePath);

            return solutionFilePath;
        }

        private string GetTemporaryDirectorySolutionFilePath()
        {
            var solutionFilePath = this.TemporaryDirectoryFilePathProvider.GetTemporaryDirectoryFilePath(TestingDataDirectoryContentConventions.ExampleVisualStudioSolutionFileNameValue);
            return solutionFilePath;
        }

        private void CreateNewSolutionFileAddFirstProject()
        {
            var solutionFilePath = this.CreateNewSolutionFileAndReturnPath();

            var solutionDirectoryPath = this.StringlyTypedPathOperator.GetDirectoryPathForFilePath(solutionFilePath);

            var (relativeProjectFilePath, projectGuidString) = Program.GetAllProjectsInformation().First();

            var projectFilePath = this.StringlyTypedPathOperator.GetFilePath(solutionDirectoryPath, relativeProjectFilePath);

            var cSharpNetStandardLibraryProjectTypeGuid = this.VisualStudioSolutionFileProjectTypeGuidProvider.GetCSharpNetStandardLibraryProjectTypeGuid();

            var projectGuid = Guid.Parse(projectGuidString);

            this.VisualStudioSolutionFileOperator.AddProjectFile(solutionFilePath, projectFilePath, cSharpNetStandardLibraryProjectTypeGuid, projectGuid);
        }

        private void CreateNewSolutionFileAddAllProjects()
        {
            var solutionFilePath = this.CreateNewSolutionFileAndReturnPath();

            var solutionDirectoryPath = this.StringlyTypedPathOperator.GetDirectoryPathForFilePath(solutionFilePath);

            var projectInformations = Program.GetAllProjectsInformation();
            foreach (var (relativeProjectFilePath, projectGuidString) in projectInformations)
            {
                var projectFilePath = this.StringlyTypedPathOperator.GetFilePath(solutionDirectoryPath, relativeProjectFilePath);

                var cSharpNetStandardLibraryProjectTypeGuid = this.VisualStudioSolutionFileProjectTypeGuidProvider.GetCSharpNetStandardLibraryProjectTypeGuid();

                var projectGuid = Guid.Parse(projectGuidString);

                this.VisualStudioSolutionFileOperator.AddProjectFile(solutionFilePath, projectFilePath, cSharpNetStandardLibraryProjectTypeGuid, projectGuid);
            }
        }

        private string GetExpendableSolutionFilePath()
        {
            var exampleSolutionFilePath = this.TestingDataDirectoryContentPathsProvider.GetExampleVisualStudioSolutionFilePath();
            var originalSolutionFilePath = this.GetTemporaryDirectorySolutionFilePath();

            var originalSolutionDirectoryPath = this.StringlyTypedPathOperator.GetDirectoryPathForFilePath(originalSolutionFilePath);

            // Hack since the solution directory path is not at least two-levels away from C:.
            var solutionDirectoryPath = this.StringlyTypedPathOperator.GetDirectoryPath(originalSolutionDirectoryPath, "Temp");

            DirectoryHelper.CreateDirectoryOkIfExists(solutionDirectoryPath);

            var solutionFileName = this.StringlyTypedPathOperator.GetFileName(originalSolutionFilePath);
            var solutionFilePath = this.StringlyTypedPathOperator.GetFilePath(solutionDirectoryPath, solutionFileName);

            File.Copy(exampleSolutionFilePath, solutionFilePath, true);

            return solutionFilePath;
        }

        private void RemoveAllProjects()
        {
            var solutionFilePath = this.GetExpendableSolutionFilePath();

            var solutionDirectoryPath = this.StringlyTypedPathOperator.GetDirectoryPathForFilePath(solutionFilePath);

            var projectInformations = Program.GetAllProjectsInformation();//.Skip(3);
            foreach (var (relativeProjectFilePath, projectGuidString) in projectInformations)
            {
                var projectFilePath = this.StringlyTypedPathOperator.GetFilePath(solutionDirectoryPath, relativeProjectFilePath);

                var success = this.VisualStudioSolutionFileOperator.RemoveProjectFile(solutionFilePath, projectFilePath);

                this.HumanOutput.WriteLine($"{projectFilePath}\n\tRemoved project file path: {success}");
            }
        }

        private SolutionFile GetSolutionFileSolutionFoldersExample()
        {
            var solutionFolderPaths = this.GetExampleSolutionFolderPaths();

            var solutionFile = this.VisualStudioSolutionFileGenerator.GenerateVisualStudioSolutionFile();

            // Add.
            foreach (var solutionFolderPath in solutionFolderPaths)
            {
                this.InMemoryVisualStudioSolutionFileOperator.AddSolutionFolder(solutionFile, solutionFolderPath);
            }

            return solutionFile;
        }

        private IEnumerable<string> GetExampleSolutionFolderPaths()
        {
            var solutionFolderPaths = new[]
            {
                "_Dependencies",
                "_Dependencies/_Dependencies",
                "_Dependencies/_Dependencies/_Dependencies/_Dependencies",
                "Others",
            };
            return solutionFolderPaths;
        }

        private IEnumerable<string> GetExampleNonExistentSolutionFolderPaths()
        {
            var nonExistentSolutionFolderPaths = new[]
            {
                "Tests",
                "_Dependencies/Tests",
                "_Dependencies/_Dependencies/Tests",
            };
            return nonExistentSolutionFolderPaths;
        }

        private void AddMultipleSolutionFolders()
        {
            var solutionFile = this.GetSolutionFileSolutionFoldersExample();

            // Test that we have all that we should have.
            var solutionFolderPaths = this.GetExampleSolutionFolderPaths();
            foreach (var solutionFolderPath in solutionFolderPaths)
            {
                var hasSolutionFolder = this.InMemoryVisualStudioSolutionFileOperator.HasSolutionFolder(solutionFile, solutionFolderPath, out _);
                if(!hasSolutionFolder)
                {
                    throw new Exception($"Should have had solution folder: {solutionFolderPath}");
                }
            }

            // Test that we don't have what we shouldn't have.
            var nonExistentSolutionFolderPaths = this.GetExampleNonExistentSolutionFolderPaths();
            foreach (var nonExistentSolutionFolderPath in nonExistentSolutionFolderPaths)
            {
                var hasSolutionFolder = this.InMemoryVisualStudioSolutionFileOperator.HasSolutionFolder(solutionFile, nonExistentSolutionFolderPath, out _);
                if (hasSolutionFolder)
                {
                    throw new Exception($"Should NOT have had solution folder: {nonExistentSolutionFolderPath}");
                }
            }

            var outputSolutionFilePath = this.GetTemporaryDirectorySolutionFilePath();

            this.VisualStudioSolutionFileSerializer.Serialize(outputSolutionFilePath, solutionFile);
        }

        private void ListRootSolutionFolders_Example()
        {
            var solutionFile = this.GetSolutionFileSolutionFoldersExample();

            var rootSolutionFolders = this.InMemoryVisualStudioSolutionFileOperator.ListRootSolutionFolders(solutionFile);

            this.HumanOutput.WriteLine("Root solution folders:");
            foreach (var rootSolutionFolder in rootSolutionFolders)
            {
                var solutionFolderName = rootSolutionFolder.ProjectName;

                this.HumanOutput.WriteLine(solutionFolderName);
            }

            this.HumanOutput.WriteBlankLine();

            foreach (var rootSolutionFolder in rootSolutionFolders)
            {
                var solutionFolderName = rootSolutionFolder.ProjectName;

                this.HumanOutput.WriteLine($"Solution folders in solution folder \"{solutionFolderName}\":");
                var solutionFolderSolutionFolders = this.InMemoryVisualStudioSolutionFileOperator.ListSolutionFolderSolutionFolders(solutionFile, solutionFolderName);
                foreach (var solutionFolderSolutionFolder in solutionFolderSolutionFolders)
                {
                    this.HumanOutput.WriteLine(solutionFolderSolutionFolder.ProjectName);
                }
            }
        }

        private void ListProjectsInSolutionFolder()
        {
            var solutionFilePath = this.TestingDataDirectoryContentPathsProvider.GetExampleVisualStudioSolutionFilePath();

            var solutionFolderName = "_Dependencies";

            var projectFilePaths = this.VisualStudioSolutionFileOperator.ListSolutionFolderProjectFilePaths(solutionFilePath, solutionFolderName);

            this.HumanOutput.WriteLine($"Project files paths in solution folder \"{solutionFolderName}\":");
            foreach (var projectFilePath in projectFilePaths)
            {
                this.HumanOutput.WriteLine(projectFilePath);
            }
        }

        private void MoveProjectsOutOfSolutionFolder()
        {
            var solutionFilePath = this.GetExpendableSolutionFilePath();

            var solutionFolderName = "_Dependencies";

            var projectFilePaths = this.VisualStudioSolutionFileOperator.ListSolutionFolderProjectFilePaths(solutionFilePath, solutionFolderName);

            foreach (var projectFilePath in projectFilePaths)
            {
                this.VisualStudioSolutionFileOperator.MoveProjectFileOutOfSolutionFolder(solutionFilePath, projectFilePath, solutionFolderName);
            }
        }

        private void RemoveSolutionFolderAndContents()
        {
            var solutionFilePath = this.GetExpendableSolutionFilePath();

            var solutionFolderName = "_Dependencies";

            this.VisualStudioSolutionFileOperator.RemoveSolutionFolderAndContents(solutionFilePath, solutionFolderName);
        }

        private void ListRootProjectFilePaths()
        {
            var solutionFilePath = this.GetExpendableSolutionFilePath();

            var rootProjectFilePaths = this.VisualStudioSolutionFileOperator.ListRootProjectFilePaths(solutionFilePath);

            this.HumanOutput.WriteLine($"Root projects of solution file:\n{solutionFilePath}");
            this.HumanOutput.WriteBlankLine();
            foreach (var rootProjectFilePath in rootProjectFilePaths)
            {
                this.HumanOutput.WriteLine(rootProjectFilePath);
            }
        }

        private void ListProjectsInSolutionFile()
        {
            var solutionFilePath = this.GetExpendableSolutionFilePath();

            var projectFilePaths = this.VisualStudioSolutionFileOperator.ListProjectReferenceFilePaths(solutionFilePath);

            this.HumanOutput.WriteLine($"Projects of solution file:\n{solutionFilePath}");
            this.HumanOutput.WriteBlankLine();
            foreach (var projectFilePath in projectFilePaths)
            {
                this.HumanOutput.WriteLine(projectFilePath);
            }
        }


        #region Main, Services, and Constructor

        static void Main(string[] args)
        {
            using (var serviceProvider = ServiceProviderServiceBuilder.New().UseStartupAndBuild<Startup>())
            {
                var program = serviceProvider.GetRequiredService<Program>();

                program.Run();
            }
        }


        private IHumanOutput HumanOutput { get; }
        private IStringlyTypedPathOperator StringlyTypedPathOperator { get; }
        private IVisualStudioSolutionFileOperator VisualStudioSolutionFileOperator { get; }
        private ITestingDataDirectoryContentPathsProvider TestingDataDirectoryContentPathsProvider { get; }
        private ITemporaryDirectoryFilePathProvider TemporaryDirectoryFilePathProvider { get; }
        private IVisualStudioSolutionFileProjectTypeGuidProvider VisualStudioSolutionFileProjectTypeGuidProvider { get; }

        private IVisualStudioSolutionFileGenerator VisualStudioSolutionFileGenerator { get; }
        private InMemoryVisualStudioSolutionFileOperator InMemoryVisualStudioSolutionFileOperator { get; }
        private IVisualStudioSolutionFileSerializer VisualStudioSolutionFileSerializer { get; }


        public Program(
            IHumanOutput humanOutput,
            IStringlyTypedPathOperator stringlyTypedPathOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            ITestingDataDirectoryContentPathsProvider testingDataDirectoryContentPathsProvider,
            ITemporaryDirectoryFilePathProvider temporaryDirectoryFilePathProvider,
            IVisualStudioSolutionFileProjectTypeGuidProvider visualStudioSolutionFileProjectTypeGuidProvider,

            IVisualStudioSolutionFileGenerator visualStudioSolutionFileGenerator,
            InMemoryVisualStudioSolutionFileOperator inMemoryVisualStudioSolutionFileOperator,
            IVisualStudioSolutionFileSerializer visualStudioSolutionFileSerializer)
        {
            this.HumanOutput = humanOutput;
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;
            this.VisualStudioSolutionFileOperator = visualStudioSolutionFileOperator;
            this.TestingDataDirectoryContentPathsProvider = testingDataDirectoryContentPathsProvider;
            this.TemporaryDirectoryFilePathProvider = temporaryDirectoryFilePathProvider;
            this.VisualStudioSolutionFileProjectTypeGuidProvider = visualStudioSolutionFileProjectTypeGuidProvider;

            this.VisualStudioSolutionFileGenerator = visualStudioSolutionFileGenerator;
            this.InMemoryVisualStudioSolutionFileOperator = inMemoryVisualStudioSolutionFileOperator;
            this.VisualStudioSolutionFileSerializer = visualStudioSolutionFileSerializer;
        }

        #endregion
    }
}
