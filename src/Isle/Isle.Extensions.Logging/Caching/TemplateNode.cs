using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Isle.Extensions.Logging.Caching;

internal sealed class TemplateNode
{
    [SkipLocalsInit]
    public TemplateNode(Node parent)
    {
        if (parent.Depth == 0)
        {
            Segments = Array.Empty<Segment>();
            MessageTemplate = string.Empty;
            return;
        }

        var nodes = ArrayPool<Node>.Shared.Rent(parent.Depth);
        var node = parent;
        while (node.Depth > 0)
        {
            nodes[node.Depth - 1] = node;
            node = node.Parent!;
        }

        var builder = new ValueStringBuilder(stackalloc char[1024]);
        var segments = new Segment[parent.Depth];
        for (int i = 0; i < parent.Depth; i++)
        {
            node = nodes[i];
            switch (node)
            {
                case LiteralNode literalNode:
                    segments[i] = new Segment(literalNode.RawLiteral);
                    builder.Append(literalNode.EscapedLiteral);
                    break;
                case HoleNode holeNode:
                    segments[i] = new Segment(null, 0);
                    builder.Append('{');
                    builder.Append(holeNode.Name);
                    builder.Append('}');
                    break;
                case FormattedHoleNode formattedHoleNode:
                    segments[i] = new Segment(formattedHoleNode.Format, formattedHoleNode.Alignment);
                    builder.Append('{');
                    builder.Append(formattedHoleNode.Name);
                    if (formattedHoleNode.Alignment != 0)
                    {
                        builder.Append(',');
                        builder.AppendSpanFormattable(formattedHoleNode.Alignment, provider: CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(formattedHoleNode.Format))
                    {
                        builder.Append(':');
                        builder.Append(formattedHoleNode.Format);
                    }
                    builder.Append('}');
                    break;
            }
        }

        Segments = segments;
        MessageTemplate = builder.ToString();
        ArrayPool<Node>.Shared.Return(nodes);
    }

    public Segment[] Segments { get; }

    public string MessageTemplate { get; }
}