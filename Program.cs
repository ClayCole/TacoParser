using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            // TODO:  Find the two Taco Bells that are the furthest from one another.
            // HINT:  You'll need two nested forloops ---------------------------

            logger.LogInfo("Log initialized");

            // use File.ReadAllLines(path) to grab all the lines from your csv file
            
            string[] lines = File.ReadAllLines(csvPath); // reads every line and converts into a string (in the TacoBell-US class)

            // Log and error if you get 0 lines and a warning if you get 1 line
            if ( lines.Length == 0)
            {
                logger.LogError("file has no data");
            }
            if (lines.Length == 1)
            {
                logger.LogWarning("file only has one input of data");
            }

            logger.LogInfo($"Lines: {lines[0]}");

            // Create a new instance of your TacoParser class
            var parser = new TacoParser();

            // Grab an IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var locations = lines.Select(line => parser.Parse(line)).ToArray();  // will read the first thing in the line and then try to Parse it
                                                                                 // turning it into a array



            // DON'T FORGET TO LOG YOUR STEPS

            // Now that your Parse method is completed, START BELOW ----------

            // TODO: Create two `ITrackable` variables with initial values of `null`. These will be used to store your two taco bells that are the farthest from each other.
            ITrackable tacoBell1 = null;  // just creating varbles for the two different TB's and one for the distance.
            ITrackable tacoBell2 = null;

            // Create a `double` variable to store the distance
            double distance = 0;

            // Include the Geolocation toolbox, so you can compare locations: `using GeoCoordinatePortable;`
            // ^^ basically asking you to confirm we have the using system above 

            //HINT NESTED LOOPS SECTION---------------------
            // Do a loop for your locations to grab each location as the origin (perhaps: `locA`)
            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];

                // Create a new corA Coordinate with your locA's lat and long
                var corA = new GeoCoordinate(); // this become out first placeholder that LocB/CorB will have to compare to 
                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;

                // Now, do another loop on the locations with the scope of your first loop, so you can grab the "destination" location (perhaps: `locB`)

                for (int x = 0; x < locations.Length; x++) // in this for loop the value of "x" will change well before the value of "i" in the other for loop.
                                                           // basically the "x" in the second for loop will go through all the indexs of thre array to compare to
                                                           // the index value capture in "i".
                {
                    // Create a new Coordinate with your locB's lat and long
                    var locB = locations[x];
                    var corB = new GeoCoordinate();
                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;

                    // Now, compare the two using `.GetDistanceTo()`, which returns a double
                    // If the distance is greater than the currently saved distance, update the distance and the two `ITrackable` variables you set above

                    if (corA.GetDistanceTo(corB) > distance)
                    {
                        distance = corA.GetDistanceTo(corB);
                        tacoBell1 = locA;
                        tacoBell2 = locB;
                            
                    }

                }

            }


            // Once you've looped through everything, you've found the two Taco Bells farthest away from each other.

            logger.LogInfo($"{tacoBell1.Name} and {tacoBell2.Name} are the farthest apart");
            var miles = distance / 1609.344;
            Console.WriteLine($"The Distance in miles is : {Math.Round(miles,2)}");
        }
    }
}
