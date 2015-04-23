using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaBD.Policy
{
    [Serializable]
    public class RequiredCommentPatternPolicy : PolicyBase
    {
        [NonSerialized]
        private IPendingCheckin mPendingCheckin;
        Regex mCommentPattern = new Regex("Task-[0-9] .+");

        public override bool CanEdit
        {
            // Policy is not configurable
            get { return false; }
        }

        public override string Description
        {
            get { return "The policy forces users to remove warnings."; }
        }

        public override string InstallationInstructions
        {
            get { return "Please contact TFS administrator"; }
        }

        public override string Type
        {
            get { return "No-Warning Policy"; }
        }

        public override string TypeDescription
        {
            get { return "This policy does not allow commit a code without empty warning list."; }
        }

        public override bool Edit(IPolicyEditArgs policyEditArgs)
        {
            // No editing options
            return true;
        }

        public override void Initialize(IPendingCheckin pendingCheckin)
        {
            base.Initialize(pendingCheckin);

            mPendingCheckin = pendingCheckin;
            mPendingCheckin.PendingChanges.CheckedPendingChangesChanged += PendingChanges_CheckedPendingChangesChanged;
        }

        void PendingChanges_CheckedPendingChangesChanged(object sender, EventArgs e)
        {
            if (!Disposed)
            {
                OnPolicyStateChanged(Evaluate());
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            mPendingCheckin.PendingChanges.CheckedPendingChangesChanged -= PendingChanges_CheckedPendingChangesChanged;
        }

        public override PolicyFailure[] Evaluate()
        {
            List<PolicyFailure> failures = new List<PolicyFailure>();

            if (!CommentMatchesPattern(mPendingCheckin.PendingChanges.Comment))
            {
                failures.Add(new PolicyFailure("Comment does not match the required pattern!", this));
            }

            return failures.ToArray();
        }

        public bool CommentMatchesPattern(string comment)
        {
            return mCommentPattern.IsMatch(comment);
        }
    }
}
