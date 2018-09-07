// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.IO;

namespace Nuke.Common.ProjectModel
{
    [PublicAPI]
    [DebuggerDisplay("{" + nameof(Path) + "}")]
    public class Project
    {
        internal Project(
            Solution solution,
            Guid projectId,
            string name,
            string path,
            Guid typeId)
        {
            Solution = solution;
            ProjectId = projectId;
            Name = name;
            Path = (PathConstruction.AbsolutePath) path;
            TypeId = typeId;
        }

        public Solution Solution { get; }
        public Guid ProjectId { get; }
        public string Name { get; }
        public PathConstruction.AbsolutePath Path { get; }
        public PathConstruction.AbsolutePath Directory => (PathConstruction.AbsolutePath) System.IO.Path.GetDirectoryName(Path).NotNull();
        public Guid TypeId { get; }

        [CanBeNull]
        public Project Parent
        {
            get => Solution.GetParentProject(this);
            set => Solution.SetParentProject(value, this);
        }

        public static implicit operator string(Project project)
        {
            return project.Path;
        }
    }
}
