variable "env" {}
variable "name" {}

resource "aws_cloudformation_stack" "upload_bucket" {
  name = "${var.name}-${var.env}-upload-bucket-stack"
  template_body = "${file("${path.module}/upload_bucket.yaml")}"
  on_failure = "DELETE"
}
