namespace BountyHuntersBlog.Constants
{
    public static class EntityConstants
    {
        public static class Faction
        {
            public const int NameMaxLength = 100;
            public const int DisplayNameMaxLength = 100;
        }
        public static class Hunter
        {
            public const int DisplayNameMaxLength = 100;
            
        }
        public static class MissionPost
        {
            public const int TitleMaxLength = 200;
            public const int PageTitleMaxLength = 200;
            public const int DescriptionMaxLength = 5000;
            public const int ContentMaxLength = 10000;
            public const int UrlHandleMaxLength = 500;
        }
        public static class MissionComment
        {
            public const int DescriptionMaxLength = 1000;
        }
    }
}
