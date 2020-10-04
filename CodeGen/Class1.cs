using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeGen
{
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // begin creating the source we'll inject into the users compilation
            StringBuilder sourceBuilder = new StringBuilder(@"
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using StyledComponents;

namespace StyledComponents 
{
    public class Styled : ComponentBase
    {
        private readonly string _control;

        public Styled(string control)
        {
            _control = control;
        }

        public Styled(Styled baseStyled) { }

        public string CssClass { get; set; }
        public string CssProperties { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var attributes = AdditionalAttributes?.ToDictionary(entry => entry.Key, entry => entry.Value)
                             ?? new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                if (attributes.ContainsKey(""class""))
                {
                    // add logic to be smarter about duplicate css classes
                    attributes[""class""] = attributes[""class""] += $"" {CssClass}"";
                }
                else
                {
                    attributes.Add(""class"", CssClass);
                }
            }

            if (!string.IsNullOrWhiteSpace(CssProperties))
            {
                if (attributes.ContainsKey(""style""))
                {
                    // add logic to be smarter about duplicate css classes
                    attributes[""style""] = attributes[""style""] += $"";{CssProperties}"";
                }
                else
                {
                    attributes.Add(""style"", CssProperties);
                }
            }

            var elementCounter = 0;
            builder.OpenElement(++elementCounter, _control);

            foreach (var (key, value) in attributes)
            {
                builder.AddAttribute(++elementCounter, key, value);
            }

            builder.AddContent(++elementCounter, this.ChildContent);
            builder.CloseElement();
        }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }
    }
}
");
            context.AddSource("helloWorldGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required
        }
    }
}
