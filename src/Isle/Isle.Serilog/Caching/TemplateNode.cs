using System.Buffers;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace Isle.Serilog.Caching;

internal sealed class TemplateNode
{
    [SkipLocalsInit]
    public TemplateNode(Node parent)
    {
        if (parent.Depth == 0)
        {
            MessageTemplate = MessageTemplate.Empty;
            PropertyNodes = Array.Empty<PropertyNode>();
            return;
        }

        int tokenCount = parent.Depth;
        var nodes = ArrayPool<Node>.Shared.Rent(tokenCount);

        try
        {
            var node = parent;

            int propertyCount = 0;
            while (node.Depth > 0)
            {
                nodes[node.Depth - 1] = node;
                if (node is PropertyNode)
                    propertyCount++;

                node = node.Parent!;
            }

            var propertyNodes = new PropertyNode[propertyCount];
            var propertyIndex = 0;

            int templateLength = parent.Offset + parent.RawText.Length;
            var vsb = templateLength <= 1024
                ? new ValueStringBuilder(stackalloc char[templateLength])
                : new ValueStringBuilder(templateLength);

            for (int i = 0; i < parent.Depth; i++)
            {
                node = nodes[i];
                vsb.Append(node.RawText);
                if (node is PropertyNode propertyNode)
                    propertyNodes[propertyIndex++] = propertyNode;
            }

            MessageTemplate = new MessageTemplate(
                vsb.ToString(),
                nodes.Select(x => x.Token).Take(tokenCount));
            PropertyNodes = propertyNodes;
        }
        finally
        {
            ArrayPool<Node>.Shared.Return(nodes);
        }
    }

    public MessageTemplate MessageTemplate { get; }

    public IReadOnlyList<PropertyNode> PropertyNodes { get; }
}