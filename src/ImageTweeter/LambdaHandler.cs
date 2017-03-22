using System.Linq;
using System.Collections.Generic;
using System.IO;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using S3Model = Amazon.S3.Model;
using Amazon.Rekognition;
using RekognitionModel = Amazon.Rekognition.Model;
using Tweetinvi;
using TweetinviModels = Tweetinvi.Models;
using Tweetinvi.Parameters;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Amazon.Rekognition.Model;

[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ImageTweeter
{
    public class LambdaHandler
    {

        private static async Task<byte[]> RetrieveBinaryPayload(string bucketName, string keyName)
        {
            using (var client = new AmazonS3Client())
            {
                using (var response = await client.GetObjectAsync(bucketName, keyName))
                using (var ms = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }

        public async Task Handler(S3Event s3Event)
        {

            // Level 1: Make this output when a file is uploaded to S3
            LambdaLogger.Log("Hello from The AutoMaTweeter!");

            // Level 2: Get the bucket name and key from event data and log to cloudwatch
            var bucketName = s3Event.Records[0].S3.Bucket.Name;
            var keyName = s3Event.Records[0].S3.Object.Key;

            LambdaLogger.Log($"The AutoMaTweeter found a file path: {bucketName}/{keyName}");

            var file1 = await RetrieveBinaryPayload(bucketName, keyName);

            // Boss Level: Use Amazon Rekognition to get keywords about the image
            IEnumerable<string> labels = Enumerable.Empty<string>();
            using (var client = new AmazonRekognitionClient())
            {
                var request = new DetectLabelsRequest();
                request.Image = new Image();
                request.Image.Bytes = new MemoryStream(file1);
                var response = await client.DetectLabelsAsync(request);
                labels = response.Labels.Select(x => x.Name);
            }

            // Level 3: Post the image and message to twitter
            var consumerKey = "GCNunS5DfXGwh8rvFAterxmXP";
            var consumerSecret = "fq03tBiAIAM7pB6DjRI8S69scCFiR3FibCbjz3HWfjEOPMLSQD";
            var accessToken = "842206967632338944-XMAH3FU86RSak57FVJmglXn4HAvNmpy";
            var accessTokenSecret = "fiFvNoEASqyqWo3FuFr5JKBYyWlILihVLlGCTSxfqAtlv";
            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            var media = Upload.UploadImage(file1);

            var message = "Team1: " + string.Join(" ", labels);
            if (message.Length > 140)
            {
                message = message.Substring(0, 139);
            }

            var tweet = Tweet.PublishTweet(message, new PublishTweetOptionalParameters
            {
                Medias = new List<IMedia> { media }
            });

        }
    }
}
