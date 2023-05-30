resource "aws_ssm_parameter" "mcce22_smart_office_userimage" {
  name  = "ImageBucketName"
  type  = "String"
  value = aws_s3_bucket.mcce22_smart_office_userimage.bucket
}

resource "aws_ssm_parameter" "mcce22_smart_office_iotdata_endpointaddress" {
  name  = "IoTDataEndpointAddress"
  type  = "String"
  value = var.mcce22_iotdata_endpointaddress
}

resource "aws_ssm_parameter" "mcce22_smart_office_smpt_host" {
  name  = "SmtpHost"
  type  = "String"
  value = var.mcce22_smpt_host
}

resource "aws_ssm_parameter" "mcce22_smart_office_smpt_port" {
  name  = "SmtpPort"
  type  = "String"
  value = var.mcce22_smpt_port
}

resource "aws_ssm_parameter" "mcce22_smart_office_smpt_username" {
  name  = "SmtpUsername"
  type  = "String"
  value = var.mcce22_smpt_username
}

resource "aws_ssm_parameter" "mcce22_smart_office_smpt_password" {
  name  = "SmtpPassword"
  type  = "String"
  value = var.mcce22_smpt_password
}

resource "aws_ssm_parameter" "mcce22_smart_office_smpt_sendername" {
  name  = "SmtpSendername"
  type  = "String"
  value = var.mcce22_smpt_sendername
}

resource "aws_ssm_parameter" "mcce22_smart_office_activator_endpointaddress" {
  name  = "ActivatorEndpointAddress"
  type  = "String"
  value = "${module.mcce22_smart_office_apigateway.api_url}/deviceactivator"
}