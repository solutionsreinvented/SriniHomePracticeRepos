using System;
using System.Collections.Generic;
using System.Linq;
using ReInvented.StaadPro.Interop.Entities;

namespace RakeMechanism.Geometry.Domain
{
    public sealed class FrameGeometry
    {
        private readonly List<Node> _allNodes;
        private readonly List<Node> _perimeterNodes;
        private readonly List<Node> _cornerNodes;
        private readonly List<int> _connectivityIndices;
        private readonly HashSet<Node> _reusedConnectivityNodes = new();

        public FrameGeometry(
            FrameProfile profile,
            IReadOnlyList<Node> allNodes,
            IReadOnlyList<Node> perimeterNodes,
            IReadOnlyList<Node> cornerNodes,
            IReadOnlyList<int> connectivityIndices)
        {
            Profile = profile;
            _allNodes = allNodes.ToList();
            _perimeterNodes = perimeterNodes.ToList();
            _cornerNodes = cornerNodes.ToList();
            _connectivityIndices = connectivityIndices.ToList();
        }

        public FrameProfile Profile { get; }

        public IReadOnlyList<Node> AllNodes => _allNodes;

        public IReadOnlyList<Node> PerimeterNodes => _perimeterNodes;

        public IReadOnlyList<Node> CornerNodes => _cornerNodes;

        public IReadOnlyList<Node> ConnectivityNodes => _connectivityIndices.Select(index => _allNodes[index]).ToArray();

        public IReadOnlyList<Node> NewlyGeneratedNodes =>
            _allNodes
                .Where(node => !_reusedConnectivityNodes.Contains(node))
                .DistinctBy(node => node.Id)
                .ToArray();

        public void ReuseConnectivityNodes(IReadOnlyList<Node> existingNodes)
        {
            if (existingNodes.Count != _connectivityIndices.Count)
            {
                throw new InvalidOperationException(
                    $"Expected {_connectivityIndices.Count} inherited connectivity nodes, but received {existingNodes.Count}.");
            }

            for (int i = 0; i < _connectivityIndices.Count; i++)
            {
                int targetIndex = _connectivityIndices[i];
                Node originalNode = _allNodes[targetIndex];
                Node replacementNode = existingNodes[i];

                ReplaceNode(originalNode, replacementNode);
                _reusedConnectivityNodes.Add(replacementNode);
            }
        }

        private void ReplaceNode(Node originalNode, Node replacementNode)
        {
            ReplaceInList(_allNodes, originalNode, replacementNode);
            ReplaceInList(_perimeterNodes, originalNode, replacementNode);
            ReplaceInList(_cornerNodes, originalNode, replacementNode);
        }

        private static void ReplaceInList(IList<Node> nodes, Node originalNode, Node replacementNode)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (ReferenceEquals(nodes[i], originalNode))
                {
                    nodes[i] = replacementNode;
                }
            }
        }
    }
}
