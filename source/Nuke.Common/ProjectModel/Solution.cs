// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Nuke.Common.ProjectModel
{
    [PublicAPI]
    public class Solution
    {
        private static readonly Guid s_solutionFolderGuid = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8");
        
        private readonly List<Project> _projects;
        private readonly Dictionary<Project, Project> _parents;

        internal Solution(string path, IEnumerable<string> header)
        {
            Path = path;
            Header = header.ToArray();
            _projects = new List<Project>();
            _parents = new Dictionary<Project, Project>();
        }

        public string Path { get; }
        public string[] Header { get; set; }
        public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

        public static implicit operator string(Solution solution)
        {
            return solution.Path;
        }

        public Project AddProject(string name, Guid typeId, string path = null, Guid? projectId = null, Project parent = null)
        {
            projectId = projectId ?? Guid.NewGuid();
            var otherProject = _projects.FirstOrDefault(x => x.ProjectId.Equals(projectId));
            ControlFlow.Assert(otherProject == null,
                $"Cannot add '{name}' because its id '{projectId}' is already taken by '{otherProject?.Name}'.");
            
            var project = new Project(this, projectId.Value, name, path ?? name, typeId);
            _projects.Add(project);
            _parents.Add(project, parent);

            return project;
        }

        /// <summary>
        /// Removes a project from a solution.
        /// </summary>
        /// <returns>The list of child projects which have bee moved upwards due to removal of the project.</returns>
        public IReadOnlyCollection<Project> RemoveProject(Project project)
        {
            var children = GetChildProjects(project).ToList();
            foreach (var child in children)
                SetParentProject(project.Parent, child);

            RemoveProject(project);
            
            return children.AsReadOnly();
        }

        [CanBeNull]
        internal Project GetParentProject(Project project)
        {
            return _parents[project];
        }

        internal IEnumerable<Project> GetChildProjects(Project project)
        {
            return _parents.Where(x => x.Value == project).Select(x => x.Key);
        }

        internal void SetParentProject([CanBeNull] Project parent, Project child)
        {
            if (parent != null)
            {
                ControlFlow.Assert(parent.Solution == child.Solution, "Projects must belong to the same solution.");
                ControlFlow.Assert(parent.ProjectId.Equals(s_solutionFolderGuid), "Parent project must be a solution folder.");
            }

            _parents[child] = parent;
        }
    }

    public static class SolutionExtensions
    {
        
        [CanBeNull]
        public static Project GetProject(this Solution solution, string wildcardPattern)
        {
            var projects = solution.GetProjects(wildcardPattern).ToList();
            ControlFlow.Assert(projects.Count <= 1, "projects.Count <= 1");
            return projects.SingleOrDefault();
        }

        public static IEnumerable<Project> GetProjects(this Solution solution, string wildcardPattern)
        {
            wildcardPattern = $"^{wildcardPattern}$";
            var regex = new Regex(wildcardPattern
                .Replace(".", "\\.")
                .Replace("*", ".*"));
            return solution.Projects.Where(x => regex.IsMatch(x.Name));
        }
    }
}
