terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.16"
    }
  }

  required_version = ">= 1.2.0"
}

provider "aws" {
  region     = var.aws_region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
  token      = var.aws_session_token
}