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

[assembly:LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ImageTweeter {
    public class LambdaHandler {
        public string Handler(ILambdaContext context) {

            // Level 1: Make this output when a file is uploaded to S3
            LambdaLogger.Log("Hello from The AutoMaTweeter!");

            // Level 2: Get the bucket name and key from event data and log to cloudwatch
            //var bucketName = '';
            //var keyName = '';

            //LambdaLogger.Log($"The AutoMaTweeter found a file path: {bucketName}/{keyName}");

            // Boss Level: Use Amazon Rekognition to get keywords about the image

            // Level 3: Post the image and message to twitter
            //var consumerKey = "GCNunS5DfXGwh8rvFAterxmXP";
            //var consumerSecret = "fq03tBiAIAM7pB6DjRI8S69scCFiR3FibCbjz3HWfjEOPMLSQD";
            //var accessToken = "842206967632338944-XMAH3FU86RSak57FVJmglXn4HAvNmpy";
            //var accessTokenSecret = "fiFvNoEASqyqWo3FuFr5JKBYyWlILihVLlGCTSxfqAtlv";

            return "Run Complete";
        }
    }
}
