env = "dev1"
name = "magicnight"
profile = "sdmagicnight"
region = "us-west-2"

terragrunt = {
  include {
    path = "${find_in_parent_folders()}"
  }
}
