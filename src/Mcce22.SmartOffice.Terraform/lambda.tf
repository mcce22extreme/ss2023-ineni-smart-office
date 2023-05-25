locals {
  dist_directory = "dist"
}

module "mcce22_smart_office_users" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.Users/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/users.zip"
  function_name         = "mcce22-smart-office-users"
  lambda_handler        = "Mcce22.SmartOffice.Users"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}

module "mcce22_smart_office_workspaces" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.Workspaces/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/workspaces.zip"
  function_name         = "mcce22-smart-office-workspaces"
  lambda_handler        = "Mcce22.SmartOffice.Workspaces"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}

module "mcce22_smart_office_device_activator" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.DeviceActivator/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/activator.zip"
  function_name         = "mcce22-smart-office-activator"
  lambda_handler        = "Mcce22.SmartOffice.DeviceActivator"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}

module "mcce22_smart_office_bookings" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.Bookings/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/bookings.zip"
  function_name         = "mcce22-smart-office-bookings"
  lambda_handler        = "Mcce22.SmartOffice.Bookings"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}

module "mcce22_smart_office_notifications" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.Notifications/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/notifications.zip"
  function_name         = "mcce22-smart-office-notifications"
  lambda_handler        = "Mcce22.SmartOffice.Notifications"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}

module "mcce22_smart_office_dataingress" {
  source                = "./modules/lambda-function"
  lambda_bucket_id      = aws_s3_bucket.mcce22_smart_office_lambda.id
  publish_dir           = "../Mcce22.SmartOffice.DataIngress/bin/Release/net6.0/linux-x64/publish"
  zip_file              = "${local.dist_directory}/dataingress.zip"
  function_name         = "mcce22-smart-office-dataingress"
  lambda_handler        = "Mcce22.SmartOffice.DataIngress::Mcce22.SmartOffice.DataIngress.Functions::HandleRequest"
  lambda_execution_role = "arn:aws:iam::064088109127:role/LabRole"
}