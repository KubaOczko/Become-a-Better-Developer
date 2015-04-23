using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaBD.Policy.Tests
{
    [TestClass]
    public class RequiredCommentPatternPolicyTests
    {
        [TestMethod]
        public void TestCommentMatch()
        {
            using (RequiredCommentPatternPolicy policy = new RequiredCommentPatternPolicy())
            {
                Assert.IsTrue(policy.CommentMatchesPattern("Task-123 Comment"));
                Assert.IsTrue(policy.CommentMatchesPattern("task-123 Comment"));

                Assert.IsFalse(policy.CommentMatchesPattern("task123"));
                Assert.IsFalse(policy.CommentMatchesPattern("task-"));
                Assert.IsFalse(policy.CommentMatchesPattern("general text"));
            }
        }
    }
}
