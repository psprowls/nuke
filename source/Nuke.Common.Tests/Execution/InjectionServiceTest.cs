// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using FakeItEasy;
using FluentAssertions;
using Nuke.Common.Execution;
using Xunit;

namespace Nuke.Common.Tests.Execution
{
    public class InjectionServiceTest
    {
        private readonly InjectionService _subject = new InjectionService();
        
        [Fact]
        public void Test()
        {
            var build = new Build();
            _subject.InjectValues(build);

            build.Field.Should().Be(nameof(Build.Field));
            build.Property.Should().Be(nameof(Build.Property));
            build.DefaultValue.Should().Be(nameof(Build));
            build.NullValue.Should().BeNull();
        }

        [Fact]
        public void TestOrdering()
        {
            var build = new OrderingBuild();
            _subject.InjectValues(build);

            A.CallTo(OrderingBuild.ParameterAction).MustHaveHappened()
                .Then(A.CallTo(OrderingBuild.InjectionAction).MustHaveHappened());
        }

        [Fact]
        public void TestTypeMismatch()
        {
            var build = new TypeMismatchBuild();
            Action action = () => _subject.InjectValues(build);

            action.Should().Throw<Exception>()
                .WithMessage("Assertion failed: Field 'Value' must be of type 'Boolean' to get its valued injected from 'ValueInjection'.");
        }

        [Fact]
        public void TestMultiple()
        {
            var build = new MultipleBuild();
            Action action = () => _subject.InjectValues(build);

            action.Should().Throw<Exception>()
                .WithMessage("Assertion failed: Member 'Injection' has multiple injection attributes applied.");
        }

        private class Build : NukeBuild
        {
            [ValueInjection(Value = nameof(Field))]
            public readonly string Field;
            
            [ValueInjection(Value = nameof(Property))]
            public readonly string Property;

            [ValueInjection(/* no value */)]
            public readonly string DefaultValue = nameof(Build);

            [ValueInjection(/* no value */)]
            public readonly int? NullValue;
            
            private class ValueInjection : InjectionAttributeBase
            {
                public object Value { get; set; }
            
                public override object GetValue(string memberName, Type memberType)
                {
                    return Value;
                }
            }
        }

        private class OrderingBuild : NukeBuild
        {
            public static readonly Action InjectionAction = A.Fake<Action>();
            public static readonly Action ParameterAction = A.Fake<Action>();

            [ValueInjection] public readonly string Injection;
            [FakeParameter] public readonly string Parameter;
            
            private class FakeParameter : ParameterAttribute
            {
                public override object GetValue(string memberName, Type memberType)
                {
                    ParameterAction();
                    return null;
                }
            }

            private class ValueInjection : InjectionAttributeBase
            {
                public override object GetValue(string memberName, Type memberType)
                {
                    InjectionAction();
                    return null;
                }
            }
        }

        private class TypeMismatchBuild : NukeBuild
        {
            [ValueInjection(Value = true)] public readonly int Value;
            
            private class ValueInjection : InjectionAttributeBase
            {
                public object Value { get; set; }
            
                public override object GetValue(string memberName, Type memberType)
                {
                    return Value;
                }
            }
        }

        private class MultipleBuild : NukeBuild
        {
            [ValueInjection] [AnotherValueInjection]
            public readonly string Injection;

            private class ValueInjection : InjectionAttributeBase
            {
                public override object GetValue(string memberName, Type memberType)
                {
                    return null;
                }
            }

            private class AnotherValueInjection : InjectionAttributeBase
            {
                public override object GetValue(string memberName, Type memberType)
                {
                    return null;
                }
            }
        }
    }
}
