using System.Windows;

namespace SupportTools_Visio.Domain
{
    public class WorkItemOffsets
    {
        public WorkItemOffset Bug;

        public WorkItemOffset Epic;

        public WorkItemOffset Feature;

        public WorkItemOffset QueryResult;

        public WorkItemOffset Release;

        public WorkItemOffset Requirement;

        public WorkItemOffset Task;

        public WorkItemOffset TestCase;

        public WorkItemOffset Unknown;

        public WorkItemOffset UserNeeds;

        public WorkItemOffset UserStory;

        public WorkItemOffsets(Point initialOffset, double height, double padX = 0.5, double padY = 0.5)
        {
            Bug = new WorkItemOffset(initialOffset, height, padX, padY);
            Epic = new WorkItemOffset(initialOffset, height, padX, padY);
            Feature = new WorkItemOffset(initialOffset, height, padX, padY);
            Release = new WorkItemOffset(initialOffset, height, padX, padY);
            Requirement = new WorkItemOffset(initialOffset, height, padX, padY);
            Task = new WorkItemOffset(initialOffset, height, padX, padY);
            TestCase = new WorkItemOffset(initialOffset, height, padX, padY);
            UserNeeds = new WorkItemOffset(initialOffset, height, padX, padY);
            UserStory = new WorkItemOffset(initialOffset, height, padX, padY);

            QueryResult = new WorkItemOffset(initialOffset, height, padX, padY);

            Unknown = new WorkItemOffset(initialOffset, 0.0);
        }
    }
}