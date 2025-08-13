namespace BountyHuntersBlog.Data.Constants
{
    public static class ModelConstants 
    {
        public static class User
        {
            public const int DisplayNameMaxLength = 50;
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class Tag
        {
            public const int NameMaxLength = 50;
        }

        public static class Mission 
        {
            public const int TitleMaxLength = 100;
            public const int DescriptionMaxLength = 1000;
        }

        public static class Comment
        {
            public const int ContentMaxLength = 500;
        }
    }
}