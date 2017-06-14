using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvlTree
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkWithAvlTree();
            Console.Read();
        }
        static void WorkWithAvlTree()
        {
            var countQuery = int.Parse(Console.ReadLine());
            StringBuilder sb = new StringBuilder();
            AVLTree avlTree = new AVLTree();
            long sum = 0;
            if (countQuery >= 1 && countQuery <= 100000)
            {
                for (int i = 0; i < countQuery; i++)
                {
                    if (i == 9)
                    {
                        var sd = 5;
                    }
                    var line = Console.ReadLine().Split(' ');
                    var queryCase = line[0];
                    switch (queryCase)
                    {
                        case "+":
                            if (long.Parse(line[1]) >= 0)
                            {
                                var addNumber = Foo(long.Parse(line[1]), sum);
                                avlTree.Add(addNumber);
                            }
                            break;
                        case "-":
                            if (long.Parse(line[1]) >= 0)
                            {
                                var deleteNumber = Foo(long.Parse(line[1]), sum);
                                avlTree.Delete(deleteNumber);
                            }
                            break;
                        case "?":
                            var searchNumber = Foo(long.Parse(line[1]), sum);
                            if (long.Parse(line[1]) >= 0)
                            {
                                var search = avlTree.Search(searchNumber);
                                if (search)
                                {
                                    sb.Append("Found" + "\n");
                                }
                                else
                                {
                                    sb.Append("Not found" + "\n");
                                }
                            }
                            else
                            {
                                sb.Append("Not found" + "\n");
                            }
                            break;
                        case "s":
                            var leftNumber = Foo(long.Parse(line[1]), sum);
                            var rightNumber = Foo(long.Parse(line[2]), sum);
                            sum = avlTree.Sum(leftNumber, rightNumber);
                            sb.Append(sum + "\n");
                            break;
                    }
                }
                Console.WriteLine(sb);
            }
        }
        static long Foo(long x, long s)
        {
            long result = (x + s) % 1000000001;
            return result;
        }
        public class AVLTree
        {
            class AVLNode
            {
                public AVLNode Left { get; set; }
                public AVLNode Right { get; set; }
                public long Value { get; set; }
                //public long Sum { get; set; }
                public int Height { get; set; }
            }

            AVLNode RootNode { get; set; }

            #region add node
            public void Add(long value)
            {
                RootNode = CreateNode(RootNode, value);
            }

            AVLNode CreateNode(AVLNode node, long value)
            {
                if (node == null)
                {
                    node = new AVLNode
                    {
                        Left = null,
                        Right = null,
                        Value = value,
                        Height = 1,
                        //Sum = value
                    };
                    return node;
                }
                if (value < node.Value)
                {
                    node.Left = CreateNode(node.Left, value);
                }
                else if (value > node.Value)
                {
                    node.Right = CreateNode(node.Right, value);
                }
                else
                {
                    return node;
                }

                node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                int balance = GetBalance(node);

                if (balance > 1 && value < node.Left.Value)
                {
                    node = RightRotation(node);
                }
                if (balance < -1 && value > node.Right.Value)
                {
                    node = LeftRotation(node);
                }
                if (balance > 1 && value > node.Left.Value)
                {
                    node = LeftRightRotation(node);
                }
                if (balance < -1 && value < node.Right.Value)
                {
                    node = RightLeftRotation(node);
                }
                //node.Sum = node.Value + GetSum(node.Left) + GetSum(node.Right);
                return node;
            }
            #endregion

            #region delete node
            public void Delete(long value)
            {
                RootNode = RemoveNode(RootNode, value);
            }
            AVLNode RemoveNode(AVLNode node, long value)
            {
                if (node == null) return node;

                if (value < node.Value)
                {
                    node.Left = RemoveNode(node.Left, value);
                }
                else if (value > node.Value)
                {
                    node.Right = RemoveNode(node.Right, value);
                }
                else
                {
                    if (node.Left == null || node.Right == null)
                    {
                        var tempNode = node.Left != null ? node.Left : node.Right;
                        if (tempNode == null)
                        {
                            tempNode = node;
                            node = null;
                        }
                        else
                        {
                            node = tempNode;
                        }
                    }
                    else
                    {
                        var tempNode = MinNode(node.Right);
                        node.Value = tempNode.Value;
                        //node.Sum = tempNode.Sum;
                        node.Right = RemoveNode(node.Right, tempNode.Value);
                    }
                }

                if (node == null) return node;

                node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                int balance = GetBalance(node);

                if (balance > 1 && GetBalance(node.Left) >= 0)
                {
                    node = RightRotation(node);
                }
                if (balance < -1 && GetBalance(node.Right) <= 0)
                {
                    node = LeftRotation(node);
                }
                if (balance > 1 && GetBalance(node.Left) < 0)
                {
                    node = LeftRightRotation(node);
                }
                if (balance < -1 && GetBalance(node.Left) > 0)
                {
                    node = RightLeftRotation(node);
                }
                //node.Sum = node.Value + GetSum(node.Left) + GetSum(node.Right);
                return node;
            }
            #endregion

            #region search node
            public bool Search(long value)
            {
                return SeacrhNode(RootNode, value);
            }
            bool SeacrhNode(AVLNode node, long value)
            {
                while (node != null)
                {
                    if (value == node.Value)
                    {
                        return true;
                    }
                    else if (value < node.Value)
                    {
                        node = node.Left;
                    }
                    else if (value > node.Value)
                    {
                        node = node.Right;
                    }
                }
                return false;
            }
            #endregion

            #region sum of set
            public long Sum(long leftValue, long rightValue)
            {
                return SumOfSet2(RootNode, leftValue, rightValue);
            }
            long SumOfSet(AVLNode node, long leftValue, long rightValue)
            {
                AVLNode LeftMinusOne = null;
                AVLNode tempNode = node;
                AVLNode temp = node;
                AVLNode result = node;
                while (node != null)
                {
                    if (leftValue == node.Value)
                    {
                        if (node.Left != null)
                        {
                            LeftMinusOne = node.Left;
                        }
                        else if (node.Right != null)
                        {
                            LeftMinusOne = node.Right;
                        }
                        break;
                    }
                    else if (leftValue < node.Value)
                    {
                        node = node.Left;
                    }
                    else if (leftValue > node.Value)
                    {
                        node = node.Right;
                    }
                }

                if (LeftMinusOne != null)
                {
                    var tupleOne = Split(node, LeftMinusOne.Value);
                    tempNode = tupleOne.Item2;
                    temp = tupleOne.Item1;
                }
                var tupleTwo = Split(tempNode, rightValue);
                result = tupleTwo.Item1;

                var t = Merge(tupleTwo.Item1, tupleTwo.Item2);
                if (t != null)
                {
                    node = Merge(temp, t);
                }

                if (result != null)
                {
                    //return result.Sum;
                    return 0;
                }
                else
                {
                    return 0;
                }
            }

            long SumOfSet2(AVLNode root, long low, long high)
            {
                if (root == null) return 0;
                if (root.Value == high && root.Value == low)
                    return root.Value;

                if (root.Value <= high && root.Value >= low)
                    return root.Value + SumOfSet2(root.Left, low, high) +
                               SumOfSet2(root.Right, low, high);

                else if (root.Value < low)
                    return SumOfSet2(root.Right, low, high);

                else return SumOfSet2(root.Left, low, high);
            }
            #endregion

            #region count nodes
            public int Count(long low, long high)
            {
                return CountNodes(RootNode, low, high);
            }
            int CountNodes(AVLNode root, long low, long high)
            {
                if (root == null) return 0;
                if (root.Value == high && root.Value == low)
                    return 1;

                if (root.Value <= high && root.Value >= low)
                    return 1 + CountNodes(root.Left, low, high) +
                               CountNodes(root.Right, low, high);

                else if (root.Value < low)
                    return CountNodes(root.Right, low, high);

                else return CountNodes(root.Left, low, high);
            }
            #endregion

            #region utils

            Tuple<AVLNode, AVLNode> Split(AVLNode node, long value)
            {
                if (node == null)
                {
                    return new Tuple<AVLNode, AVLNode>(null, null);
                }
                if (value < node.Value)
                {
                    var tempTuple = Split(node.Left, value);
                    if (tempTuple.Item2 != null)
                    {
                        var tempNode = AVLMergeWithRoot(tempTuple.Item2, node.Right, node);
                        return new Tuple<AVLNode, AVLNode>(tempTuple.Item1, tempNode);
                    }
                    else
                    {
                        return new Tuple<AVLNode, AVLNode>(node, null);
                    }
                }
                else
                {
                    var tempTuple = Split(node.Right, value);
                    if (tempTuple.Item1 != null)
                    {
                        var tempNode = AVLMergeWithRoot(tempTuple.Item1, node.Left, node);
                        return new Tuple<AVLNode, AVLNode>(tempTuple.Item2, tempNode);
                    }
                    else
                    {
                        return new Tuple<AVLNode, AVLNode>(node, null);
                    }
                }
            }
            AVLNode MergeWithRoot(AVLNode v1, AVLNode v2, AVLNode t)
            {
                t.Left = v1;
                t.Right = v2;
                return t;
            }
            AVLNode AVLMergeWithRoot(AVLNode v1, AVLNode v2, AVLNode t)
            {
                var tempHeight = (v1.Height - v2.Height) < 0 ? (v1.Height - v2.Height) * -1 : (v1.Height - v2.Height);
                if (tempHeight <= 1)
                {
                    t = MergeWithRoot(v1, v2, t);
                    t.Height = maxInt(GetHeight(t.Left), GetHeight(t.Right)) + 1;
                    //t.Sum = t.Value + GetSum(t.Left) + GetSum(t.Right);
                    return t;
                }
                else if (v1.Height > v2.Height)
                {
                    var tempNode = AVLMergeWithRoot(v1.Right, v2, t);
                    v1.Right = tempNode;

                    v1.Height = maxInt(GetHeight(v1.Left), GetHeight(v1.Right)) + 1;
                    int balance = GetBalance(v1);
                    if (balance < -1 && GetBalance(v1.Right) <= 0)
                    {
                        v1 = LeftRotation(v1);
                    }
                    if (balance < -1 && GetBalance(v1.Left) > 0)
                    {
                        v1 = RightLeftRotation(v1);
                    }
                    //v1.Sum = v1.Value + GetSum(v1.Left) + GetSum(v1.Right);

                    return v1;
                }
                else
                {
                    var tempNode = AVLMergeWithRoot(v1.Left, v1, t);
                    v2.Left = tempNode;

                    v2.Height = maxInt(GetHeight(v2.Left), GetHeight(v2.Right)) + 1;
                    int balance = GetBalance(v2);

                    if (balance > 1 && GetBalance(v2.Left) >= 0)
                    {
                        v2 = RightRotation(v2);
                    }
                    if (balance > 1 && GetBalance(v2.Left) < 0)
                    {
                        v2 = LeftRightRotation(v2);
                    }
                    //v2.Sum = v2.Value + GetSum(v2.Left) + GetSum(v2.Right);

                    return v2;
                }
            }
            AVLNode Merge(AVLNode v1, AVLNode v2)
            {
                if (v1 != null)
                {
                    var t = MaxNode(v1);
                    Delete(t.Value);
                    t = MergeWithRoot(v1, v2, t);
                    return t;
                }
                else
                {
                    return v2;
                }

            }
            int GetHeight(AVLNode node)
            {
                return node != null ? node.Height : 0;
            }
            int GetBalance(AVLNode node)
            {
                if (node == null) return 0;
                return GetHeight(node.Left) - GetHeight(node.Right);
            }
            //long GetSum(AVLNode node)
            //{
            //    return node != null ? node.Sum : 0;
            //}
            AVLNode RightRotation(AVLNode node)
            {
                //AVLNode temp = node.Left;
                //node.Left = temp.Right;
                //temp.Right = node;

                //node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                //temp.Height = maxInt(GetHeight(temp.Left), node.Height) + 1;
                //node.Sum = node.Value + GetSum(node.Left) + GetSum(node.Right);
                //return temp;
                AVLNode temp = node.Left;
                AVLNode temp1 = temp.Right;

                temp.Right = node;
                node.Left = temp1;

                node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                temp.Height = maxInt(GetHeight(temp.Left), GetHeight(temp.Right)) + 1;

                return temp;
            }
            AVLNode LeftRotation(AVLNode node)
            {
                //AVLNode temp = node.Right;
                //node.Right = temp.Left;
                //temp.Left = node;

                //node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                //temp.Height = maxInt(GetHeight(temp.Right), node.Height) + 1;
                //node.Sum = node.Value + GetSum(node.Left) + GetSum(node.Right);
                //return temp;

                AVLNode temp = node.Right;
                AVLNode temp1 = temp.Left;

                temp.Left = node;
                node.Right = temp1;

                node.Height = maxInt(GetHeight(node.Left), GetHeight(node.Right)) + 1;
                temp.Height = maxInt(GetHeight(temp.Left), GetHeight(temp.Right)) + 1;

                return temp;
            }
            AVLNode LeftRightRotation(AVLNode node)
            {
                node.Left = LeftRotation(node.Left);
                return RightRotation(node);
            }
            AVLNode RightLeftRotation(AVLNode node)
            {
                node.Right = RightRotation(node.Right);
                return LeftRotation(node);
            }
            AVLNode MaxNode(AVLNode node)
            {
                if (node.Right != null)
                {
                    node = MaxNode(node.Right);
                }
                return node;
            }
            AVLNode MinNode(AVLNode node)
            {
                //if (node.Left != null)
                //{
                //    node = MinNode(node.Left);
                //}
                //return node;
                AVLNode current = node;

                /* loop down to find the leftmost leaf */
                while (current.Left != null)
                    current = current.Left;

                return current;
            }
            int maxInt(int a, int b)
            {
                return a > b ? a : b;
            }
            #endregion
        }
    }
}
