variable "aws_region" {
  type        = string
  default     = "us-east-1"
  description = "The region to deploy resources to."
}

variable "aws_access_key_id" {
  type        = string
  description = "The AWS access key."
}

variable "aws_secret_access_key" {
  type        = string
  description = "The AWS secret."
}

variable "aws_session_token" {
  type        = string
  description = "The AWS session token."
}

variable "mcce22_tf_state_bucket" {
  type        = string
  description = "The AWS S3 bucket to store terraform state."
}

variable "mcce22_smpt_host" {
  type        = string
  description = "The smtp host that is used to send emails."
}

variable "mcce22_smpt_port" {
  type        = string
  description = "The smtp port that is used to send emails."
}

variable "mcce22_smpt_username" {
  type        = string
  description = "The username that is used to authenticate on the smtp server."
}

variable "mcce22_smpt_password" {
  type        = string
  description = "The password that is used to authenticate on the smtp server."
}

variable "mcce22_smpt_sendername" {
  type        = string
  description = "The sender name that is used to send emails."
}

variable "mcce22_iotdata_endpointaddress" {
  type        = string
  default     = "https://data-ats.iot.us-east-1.amazonaws.com"
  description = "The address of AWS IoT Core data endpoint."
}

variable "mcce22_iam_role" {
  type        = string
  description = "The AWS IAM role that should be used for lambda execution."
}
