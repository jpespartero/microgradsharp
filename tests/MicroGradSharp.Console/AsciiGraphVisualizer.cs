using System;
using System.Collections.Generic;
using System.Text;

namespace MicroGradSharp.Console
{
    public static class AsciiGraphVisualizer
    {
        public static void PrintAsciiGraph(Value rootNode)
        {
            var visited = new HashSet<Value>();
            var nodeMap = new Dictionary<Value, string>();
            var idCounter = 0;
            
            // First pass: assign IDs to nodes
            AssignIds(rootNode, nodeMap, ref idCounter, visited);
            
            // Second pass: print the graph
            visited.Clear();
            PrintNode(rootNode, nodeMap, visited, new StringBuilder(), true);
        }
        
        private static void AssignIds(Value node, Dictionary<Value, string> nodeMap, ref int idCounter, HashSet<Value> visited)
        {
            if (visited.Contains(node))
                return;
                
            visited.Add(node);
            nodeMap[node] = $"[{idCounter++}]";
            
            if (node.Previous != null)
            {
                foreach (var child in node.Previous)
                {
                    if (child != null)
                    {
                        AssignIds(child, nodeMap, ref idCounter, visited);
                    }
                }
            }
        }
        
        private static void PrintNode(Value node, Dictionary<Value, string> nodeMap, HashSet<Value> visited, 
            StringBuilder indent, bool isLast)
        {
            if (visited.Contains(node))
            {
                System.Console.WriteLine($"{indent}└── {nodeMap[node]} (already visited)");
                return;
            }
            
            visited.Add(node);
            
            string nodeInfo = $"{nodeMap[node]} Value={node.Data:0.00}, Grad={node.Grad:0.00}";
            if (node.Operation != null && !string.IsNullOrEmpty(node.Operation.ToString()))
                nodeInfo += $", Op={(node.Operation != null ? node.Operation.ToString() : "None")}";
            
            System.Console.WriteLine($"{indent}{(isLast ? "└── " : "├── ")}{nodeInfo}");
            
            if (node.Previous != null && node.Previous?.Count() > 0)
            {
                var newIndent = new StringBuilder(indent.ToString());
                newIndent.Append(isLast ? "    " : "│   ");
                
                for (int i = 0; i < node.Previous.Count(); i++)
                {
                    var child = node.Previous[i];
                    if (child != null)
                    {
                        bool childIsLast = (i == node.Previous.Count() - 1);
                        PrintNode(child, nodeMap, visited, newIndent, childIsLast);
                    }
                }
            }
        }
    }
}