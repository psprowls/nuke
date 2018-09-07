// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Common.ProjectModel
{
    [PublicAPI]
    public static class ProjectModelTasks
    {
        public static Solution ParseSolution(string solutionFile)
        {
            string GuidPattern(string text)
                => $@"\{{(?<{Regex.Escape(text)}>[0-9a-fA-F]{{8}}-[0-9a-fA-F]{{4}}-[0-9a-fA-F]{{4}}-[0-9a-fA-F]{{4}}-[0-9a-fA-F]{{12}})\}}";

            string TextPattern(string name)
                => $@"""(?<{Regex.Escape(name)}>[^""]*)""";

            string ProjectPattern()
                => $@"^Project\(""{GuidPattern("typeId")}""\)\s*=\s*{TextPattern("name")},\s*{TextPattern("path")},\s*""{GuidPattern("id")}""$";

            var lines = File.ReadAllLines(solutionFile);

            var header = lines.TakeWhile(x => !x.StartsWith("Project")).ToArray();
            var solution = new Solution(solutionFile, header);

            var projectData = lines
                .Select(x => Regex.Match(x, ProjectPattern()))
                .Where(x => x.Success)
                .Select(x =>
                    new
                    {
                        ProjectId = Guid.Parse(x.Groups["id"].Value),
                        Name = x.Groups["name"].Value,
                        TypeId = Guid.Parse(x.Groups["typeId"].Value),
                        Path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solutionFile).NotNull(), x.Groups["path"].Value))
                    }).ToList();
            var projects = projectData.Select(x => solution.AddProject(x.Name, x.TypeId, x.Path, x.ProjectId)).ToList();
            
            var childToParent = lines
                .SkipWhile(x => !Regex.IsMatch(x, @"^\s*GlobalSection\(NestedProjects\) = preSolution$"))
                .Skip(count: 1)
                .TakeWhile(x => !Regex.IsMatch(x, @"^\s*EndGlobalSection$"))
                .Select(x => Regex.Match(x, $@"^\s*{GuidPattern("child")}\s*=\s*{GuidPattern("parent")}$"))
                .ToDictionary(x => Guid.Parse(x.Groups["child"].Value), x => Guid.Parse(x.Groups["parent"].Value));
            foreach (var (child, parent) in childToParent)
            {
                var childProject = projects.SingleOrDefault(x => x.ProjectId == child).NotNull("childProject != null");
                var parentProject = projects.SingleOrDefault(x => x.ProjectId == parent).NotNull("parentProject != null");
                solution.SetParentProject(parentProject, childProject);
            }

            return solution;
        }
    }
}
