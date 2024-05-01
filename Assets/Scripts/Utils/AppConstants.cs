using System.Collections.Generic;

public static class AppConstants
{
    public static class UIConstants
    {
        public const string POEMBYAI = "Poem\nby AI";
        public const string FINDINYOURSPACE = "Find in Your Space";
        public const string HURRAY = "Hurray!!";
        public const string OOPS = "OOPS!";
        public const string RETRY = "Retry";
    }
    public class PlayerPrefConstants
    {
        public const string GUIDEINDEX = "GuideIndex";
    }
    public static class NetworkConstants
    {
        public static List<string> POEMWORDS = new() { "Summer", "Sports", "Snow", "Picnic", "Rain", "Festival", "Food", "Adventure", "Celebration", "Victory", "Parade", "Play", "Office", "Home", "Vehicles", "Gadgets", "Trip", "Fun", "Childhood", "Vacation" };
        public static string[] LOADINGWORDS = new string[]
        {
            "Crafting verses from the whispers of inspiration...",
            "Gathering stardust to weave into poetic tapestries...",
            "Summoning the muse to dance upon the blank canvas...",
            "Navigating the labyrinth of creativity to find the perfect words...",
            "Filling the silence with the melody of poetic expression...",
            "Wandering through the corridors of imagination, seeking poetic treasures...",
            "Embracing the chaos of creativity to sculpt poetic beauty...",
            "Harvesting the blooms of inspiration to adorn the page with verse...",
            "Harnessing the winds of creativity to sail into the realm of poetry...",
            "Planting seeds of imagination to bloom into poetic gardens...",
            "Diving into the ocean of thought to retrieve pearls of poetry...",
            "Awaiting the symphony of words to orchestrate a poetic masterpiece...",
            "Embarking on a journey through the corridors of the mind to unlock poetic secrets...",
            "Stirring the cauldron of creativity to brew a potion of poetic magic...",
            "Harnessing the stars to guide the pen in crafting celestial verses...",
            "Treading softly through the garden of imagination, plucking petals of inspiration...",
            "Firing up the engines of imagination to soar into the realm of verse...",
            "Sifting through the sands of time to uncover forgotten poetic treasures...",
            "Kindling the flames of creativity to illuminate the path to poetic revelation...",
            "Waiting for the ink to flow and paint the canvas of thought with poetic hues..."
        };
        public const string TEXTAPI = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=";
        public const string FAILTOLOAD = "Fail to load poem";
    }

}