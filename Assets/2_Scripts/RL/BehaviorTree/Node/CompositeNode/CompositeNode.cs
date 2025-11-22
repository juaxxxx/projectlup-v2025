using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.RL
{
    public abstract class CompositeNode : Node
    {
        [SerializeField]
        protected int currWorkingNodeIndex = 0;

        protected List<Node> childListNodes = new List<Node>();
        protected CompositeNode(List<Node> childlist)
        {
            childListNodes = childlist;
        }
    }
}

