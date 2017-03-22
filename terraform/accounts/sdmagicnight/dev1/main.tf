variable "env" {}
variable "name" {}
variable "profile" {}
variable "region" {}

provider "aws" {
  profile = "${var.profile}"
  region  = "${var.region}"
}

module "sdmagicnight" {
  env = "dev1"
  name = "magicnight"
  source = "../../../modules/sdmagicnight"
}
