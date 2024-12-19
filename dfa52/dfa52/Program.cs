using System;
using System.Collections.Generic;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree bst = new BinarySearchTree();

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить узел");
                Console.WriteLine("2. Удалить узел");
                Console.WriteLine("3. Найти узел");
                Console.WriteLine("4. Подсчитать количество узлов");
                Console.WriteLine("5. Найти высоту дерева");
                Console.WriteLine("6. Найти минимальное значение");
                Console.WriteLine("7. Найти максимальное значение");
                Console.WriteLine("8. Проверить сбалансированность дерева");
                Console.WriteLine("9. Отобразить дерево как список");
                Console.WriteLine("10. Найти родителя узла");
                Console.WriteLine("11. Копировать дерево");
                Console.WriteLine("12. Объединить два дерева");
                Console.WriteLine("0. Выйти");

                int choice = int.Parse(Console.ReadLine() ?? "0");

                switch (choice)
                {
                    case 1:
                        Console.Write("Введите значение для добавления: ");
                        int addValue = int.Parse(Console.ReadLine());
                        bst.Add(addValue);
                        break;
                    case 2:
                        Console.Write("Введите значение для удаления: ");
                        int deleteValue = int.Parse(Console.ReadLine());
                        bst.Remove(deleteValue);
                        break;
                    case 3:
                        Console.Write("Введите значение для поиска: ");
                        int searchValue = int.Parse(Console.ReadLine());
                        Console.WriteLine(bst.Find(searchValue) ? "Узел найден" : "Узел не найден");
                        break;
                    case 4:
                        Console.WriteLine($"Количество узлов: {bst.CountNodes()}");
                        break;
                    case 5:
                        Console.WriteLine($"Высота дерева: {bst.Height()}");
                        break;
                    case 6:
                        Console.WriteLine($"Минимальное значение: {bst.FindMin()}");
                        break;
                    case 7:
                        Console.WriteLine($"Максимальное значение: {bst.FindMax()}");
                        break;
                    case 8:
                        Console.WriteLine(bst.IsBalanced() ? "Дерево сбалансировано" : "Дерево несбалансировано");
                        break;
                    case 9:
                        Console.WriteLine("Дерево как список: " + string.Join(", ", bst.ToList()));
                        break;
                    case 10:
                        Console.Write("Введите значение для поиска родителя: ");
                        int parentValue = int.Parse(Console.ReadLine());
                        var parent = bst.FindParent(parentValue);
                        Console.WriteLine(parent == null ? "Родитель не найден" : $"Родитель: {parent.Value}");
                        break;
                    case 11:
                        var copiedTree = bst.Copy();
                        Console.WriteLine("Дерево скопировано.");
                        break;
                    case 12:
                        Console.WriteLine("Создайте второе дерево для объединения.");
                        BinarySearchTree secondTree = new BinarySearchTree();
                        Console.WriteLine("Добавьте значения в новое дерево, введите 0 для завершения.");
                        while (true)
                        {
                            int newValue = int.Parse(Console.ReadLine());
                            if (newValue == 0) break;
                            secondTree.Add(newValue);
                        }
                        bst.Merge(secondTree);
                        Console.WriteLine("Деревья объединены.");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }
    }

    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(int value)
        {
            Value = value;
        }
    }

    public class BinarySearchTree
    {
        private TreeNode root;

        public void Add(int value)
        {
            root = AddRecursive(root, value);
        }

        private TreeNode AddRecursive(TreeNode node, int value)
        {
            if (node == null) return new TreeNode(value);

            if (value < node.Value)
                node.Left = AddRecursive(node.Left, value);
            else if (value > node.Value)
                node.Right = AddRecursive(node.Right, value);

            return node;
        }

        public bool Find(int value)
        {
            return FindRecursive(root, value);
        }

        private bool FindRecursive(TreeNode node, int value)
        {
            if (node == null) return false;

            if (value == node.Value) return true;

            return value < node.Value
                ? FindRecursive(node.Left, value)
                : FindRecursive(node.Right, value);
        }

        public void Remove(int value)
        {
            root = RemoveRecursive(root, value);
        }

        private TreeNode RemoveRecursive(TreeNode node, int value)
        {
            if (node == null) return null;

            if (value < node.Value)
                node.Left = RemoveRecursive(node.Left, value);
            else if (value > node.Value)
                node.Right = RemoveRecursive(node.Right, value);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                TreeNode minLargerNode = FindMinNode(node.Right);
                node.Value = minLargerNode.Value;
                node.Right = RemoveRecursive(node.Right, minLargerNode.Value);
            }

            return node;
        }

        private TreeNode FindMinNode(TreeNode node)
        {
            return node.Left == null ? node : FindMinNode(node.Left);
        }

        public int FindMin()
        {
            if (root == null) throw new InvalidOperationException("Дерево пусто.");
            return FindMinNode(root).Value;
        }

        public int FindMax()
        {
            if (root == null) throw new InvalidOperationException("Дерево пусто.");
            TreeNode current = root;
            while (current.Right != null)
                current = current.Right;
            return current.Value;
        }

        public int CountNodes()
        {
            return CountNodesRecursive(root);
        }

        private int CountNodesRecursive(TreeNode node)
        {
            if (node == null) return 0;
            return 1 + CountNodesRecursive(node.Left) + CountNodesRecursive(node.Right);
        }

        public int Height()
        {
            return HeightRecursive(root);
        }

        private int HeightRecursive(TreeNode node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(HeightRecursive(node.Left), HeightRecursive(node.Right));
        }

        public bool IsBalanced()
        {
            return IsBalancedRecursive(root);
        }
        private bool IsBalancedRecursive(TreeNode node)
        {
            if (node == null) return true;

            int leftHeight = HeightRecursive(node.Left);
            int rightHeight = HeightRecursive(node.Right);

            return Math.Abs(leftHeight - rightHeight) <= 1
                && IsBalancedRecursive(node.Left)
                && IsBalancedRecursive(node.Right);
        }

        public List<int> ToList()
        {
            List<int> result = new List<int>();
            InOrderTraversal(root, result);
            return result;
        }

        private void InOrderTraversal(TreeNode node, List<int> result)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, result);
            result.Add(node.Value);
            InOrderTraversal(node.Right, result);
        }

        public TreeNode FindParent(int value)
        {
            return FindParentRecursive(root, null, value);
        }

        private TreeNode FindParentRecursive(TreeNode node, TreeNode parent, int value)
        {
            if (node == null) return null;

            if (node.Value == value) return parent;

            return value < node.Value
                ? FindParentRecursive(node.Left, node, value)
                : FindParentRecursive(node.Right, node, value);
        }

        public BinarySearchTree Copy()
        {
            BinarySearchTree newTree = new BinarySearchTree();
            CopyRecursive(root, newTree);
            return newTree;
        }

        private void CopyRecursive(TreeNode node, BinarySearchTree newTree)
        {
            if (node == null) return;
            newTree.Add(node.Value);
            CopyRecursive(node.Left, newTree);
            CopyRecursive(node.Right, newTree);
        }

        public void Merge(BinarySearchTree otherTree)
        {
            foreach (var value in otherTree.ToList())
            {
                Add(value);
            }
        }
    }
}