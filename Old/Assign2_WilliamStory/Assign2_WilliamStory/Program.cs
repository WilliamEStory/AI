using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign2_WilliamStory
{
    class Program
    {
        static Node parentRoot; //Used to get the local parent root since each node doesn't provide a link
        //to its parent.
        static Random leaf = new Random();
        static int searchCost;
        static int indent = 0;
        static bool finishedTree = false;
        static void Main(string[] args)
        {
            searchCost = 0;

            Console.Out.WriteLine("Input file location");
            //string filePath = Console.ReadLine();
            string filePath = "C:\\Users\\William_2\\Documents\\visual studio 2012\\Projects\\Assign2_WilliamStory\\Assign2_WilliamStory\\TestXML.txt";
            StreamReader fileReader = new StreamReader(filePath);

            //Create a root node to pass to the parser I made.
            Node rootNode = new Node("root", "root");
            buildXmlTree(fileReader, rootNode);

            //do searching here
            string behavior = "";
            Console.Out.WriteLine("Input a behavior");
            Console.Out.WriteLine("Type \"Quit\" to exit");
            Console.Out.WriteLine();

            while (!(behavior.ToLower().Equals("quit".ToLower())))
            {
                bool found = false;
                behavior = Console.ReadLine();
                if (behavior.Trim().Equals(string.Empty))
                {
                    Console.Out.WriteLine("No event was entered");
                }
                else
                {
                    searchCost = 0;
                    //breadthFirstSearch(rootNode, behavior, out found);
                    depthFirstSearch(rootNode, behavior, out found);
                    if (found)
                    {
                        Console.Out.WriteLine("");
                    }
                    else
                    {
                        Console.Out.WriteLine("Event not found.\n");
                    }
                }
            }
        }


        #region BUILDTREE_DEPTHSEARCH_BREADTHSEARCH

        static void buildXmlTree(StreamReader xmlFile, Node root)
        {
            if(finishedTree)
            {
                return;
            }

            //Read the line.
            string currentLine = xmlFile.ReadLine();
            if (currentLine == null)
            {
                return;
            }
            else if (!(currentLine.Equals(string.Empty)) && currentLine.Length > 4)
            {
                //get the current indent of the line to make printing out the 
                //tree have a nicer format.
                indent = 0;
                for (int i = 0; i < currentLine.Length; i++)
                {
                    if (currentLine[i].Equals(' '))
                    {
                        indent++;
                    }
                    else
                    {
                        break;
                    }
                }
                indent = indent + 20;

                //Get the initial tag, either root, /root, node, or /node.
                string getTag = currentLine.Trim().Substring(1, 4);

                //if the tag signifies an to a node such as /root or /node, then adjust the tag string.
                if (getTag[0].Equals('/'))
                {
                    getTag = currentLine.Trim().Substring(1, 5);
                }

                //Console.Out.WriteLine("   " + getTag);


                if (getTag.ToLower().Equals("root"))
                {
                    Console.Out.WriteLine("BEHAVIOR = ROOT");
                    buildXmlTree(xmlFile, root);
                }
                else if (getTag.ToLower().Equals("node"))
                {
                    //get behavior and reponse from the current line.
                    string behavior = getBehavior(currentLine);
                    string response = getResponse(currentLine);

                    //make a child node, and add to its parent list.
                    Node childNode = new Node(behavior, response);
                    root.childList.Add(childNode);

                    //Get the end tag of the line, so we know if we should
                    //continue with our current child or move back up to the parent node.
                    string endTag = currentLine.Substring(currentLine.Length - 2, 2);
                    if (endTag.Equals("/>"))
                    {
                        Console.Out.WriteLine("RESPONSE = ".PadLeft(indent) + childNode.response);
                        buildXmlTree(xmlFile, root);
                    }
                    else
                    {
                        parentRoot = root;
                        Console.Out.WriteLine("BEHAVIOR = ".PadLeft(15) + childNode.behavior);
                        buildXmlTree(xmlFile, childNode);
                    }
                }
                else if (getTag.Equals("/node"))
                {
                    //the node is closed off, so we have to go back up to 
                    //the local parent node in the tree.
                    buildXmlTree(xmlFile, parentRoot);
                }
                else if (getTag.Equals("/root"))
                {
                    finishedTree = true;
                    return;
                }
                else
                {
                    buildXmlTree(xmlFile, root);
                }
            }
            else
            {
                buildXmlTree(xmlFile, root);
            }
        }

        static void breadthFirstSearch(Node treeRoot, string behavior, out bool found)
        {
            found = false;
            Queue<Node> search = new Queue<Node>();
            search.Enqueue(treeRoot);
            while (search.Count != 0)
            {
                searchCost++;
                Node current = search.Dequeue();
                if (current.behavior.ToLower().Equals(behavior.ToLower()))
                {
                    found = true;
                    //clear the queue since we are entering a sub-tree.
                    search.Clear();
                    while (current.childList.Count > 0)
                    {
                        searchCost++;
                        foreach (Node childLeaf in current.childList)
                        {
                            //add all children to the tree.
                            search.Enqueue(childLeaf);
                        }
                        if (search.Count == 1)
                        {
                            current = search.Dequeue();
                        }
                        else
                        {
                            //generate a random number that will be used to
                            //pop off a number of leafs to get to a "random"
                            //response.
                            int child = leaf.Next(current.childList.Count);
                            for (int i = 0; i < child; i++)
                            {
                                search.Dequeue();
                            }
                            current = search.Dequeue();
                        }
                    }
                    //write out the response.
                    Console.Out.WriteLine("Reponse: " + current.response);
                    Console.Out.WriteLine("Search cost: " + searchCost);
                    searchCost = 0;
                    break;
                }
                else
                {
                    foreach (Node child in current.childList)
                    {
                        search.Enqueue(child);
                    }
                }
            }
        }

        static void depthFirstSearch(Node treeRoot, string behavior, out bool found)
        {
            found = false;
            Stack<Node> search = new Stack<Node>();
            search.Push(treeRoot);
            while (search.Count > 0)
            {
                searchCost++;
                Node current = search.Pop();
                if (current.behavior.ToLower().Equals(behavior.ToLower()))
                {
                    found = true;
                    //We found the behavior we want. So go into the subtree.
                    search.Clear();
                    while (current.childList.Count > 0)
                    {
                        searchCost++;
                        foreach (Node childLeaf in current.childList)
                        {
                            search.Push(childLeaf);
                        }
                        if (search.Count == 1)
                        {
                            current = search.Pop();
                        }
                        else
                        {
                            int child = leaf.Next(current.childList.Count);
                            for (int i = 0; i < child; i++)
                            {
                                search.Pop();
                            }
                            current = search.Pop();
                        }
                    }
                    Console.Out.WriteLine("Reponse: " + current.response);
                    Console.Out.WriteLine("Search Cost: " + searchCost);
                    searchCost = 0;
                    break;
                }
                else
                {
                    //Keep searching
                    foreach (Node child in current.childList)
                    {
                        search.Push(child);
                    }
                }
            }
        }

        #endregion

        #region HELPER_FUNCTIONS

        static string getBehavior(string currLine)
        {
            int location = currLine.ToLower().LastIndexOf("behavior=\"".ToLower());
            string behavior = currLine.Substring(location + "behavior=\"".Length);
            behavior = behavior.Substring(0, behavior.IndexOf("\""));
            return behavior;
        }

        static string getResponse(string currLine)
        {
            string response = "";
            int location = currLine.ToLower().LastIndexOf("response=\"".ToLower());
            if (location == -1)
            {
                response = "";
            }
            else
            {
                response = currLine.Substring(location + "response=\"".Length);
                response = response.Substring(0, response.IndexOf("\""));
            }

            return response;
        }

        #endregion
    }
}
