variable "aws_region" {
  type        = string
  default     = "us-east-1"
  description = "The region to deploy resources to."
}

variable "aws_access_key_id" {
  type        = string
  default     = "ASIAQ527S6BDXX6U3O7D"
  description = "The AWS access key."
}

variable "aws_secret_access_key" {
  type        = string
  default     = "eox38bVMhik7+YQAIkO2dB4c37YySRyivFv57Mfz"
  description = "The AWS secret."
}

variable "aws_session_token" {
  type        = string
  default     = "FwoGZXIvYXdzEM///////////wEaDK+rQ24lx8+PC9GttyLAAcngCyyY9dsAZbYiADYwDrL3ugcSKPxcEWpFtVH3n8/oQbdrf4qSczhaFH8YE1hKvIQkTFLMOxVLHr0dIM0CXNBNio9rBprzN2uLTVtmHy4KEzzOxyWwUCvON9/fZ7K0VrA18Ugj0Vy574q42sXL05MVSX4jFm9PBoWqe1yfFnnsAFgC06Qtw+xoOksVMNwwgPUFmfYWHPyVajL81AcOtrJ+g9gLvw4wWu6sIIS6Fx817b80pMdCV5EkIoc9+//oWCjsxrajBjItJcLX+DbrbBKYi/P2cmx/d+XHItP1ECqEiaoh5KTJx8JNKRexS0EX+pgP5I9n"
  description = "The AWS session token."
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
