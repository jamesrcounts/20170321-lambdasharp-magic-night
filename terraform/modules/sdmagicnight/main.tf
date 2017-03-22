variable "env" {}
variable "name" {}

data "archive_file" "lambda_zip" {
  output_path = "../../../tmp/lambda.zip"
  source_dir = "../../../../src/ImageTweeter/bin/Debug/netcoreapp1.0/publish"
  type = "zip"
}

resource "aws_cloudformation_stack" "upload_bucket" {
  name = "${var.name}-${var.env}-upload-bucket-stack"
  template_body = "${file("${path.module}/upload_bucket.yaml")}"
  on_failure = "DELETE"
}

resource "aws_s3_bucket_object" "lambda_zip" {
  bucket = "${aws_cloudformation_stack.upload_bucket.outputs["UploadBucket"]}"
  depends_on = ["data.archive_file.lambda_zip"]
  etag = "${md5(file("../../../tmp/lambda.zip"))}"
  key = "lambda.zip"
  source = "../../../tmp/lambda.zip"
}

resource "aws_cloudformation_stack" "lambda_function" {
  capabilities = ["CAPABILITY_IAM"]
  name = "${var.name}-${var.env}-lambda-function-stack"
  template_body = "${file("${path.module}/lambda.yaml")}"
  parameters {
    LambdaBucket = "${aws_cloudformation_stack.upload_bucket.outputs["UploadBucket"]}"
  }
  on_failure = "DELETE"
}
