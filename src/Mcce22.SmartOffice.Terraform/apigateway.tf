module "mcce22_smart_office_apigateway" {
  source            = "./modules/api-gateway"
  api_name          = "mcce-smart-office-api"
  stage_name        = "dev"
  stage_auto_deploy = true
}

module "mcce22_smart_office_users_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_users.function_arn
  function_name = module.mcce22_smart_office_users.function_name
  http_method   = "ANY"
  route         = "/user"
}

module "mcce22_smart_office_users_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_users.function_arn
  function_name = module.mcce22_smart_office_users.function_name
  http_method   = "ANY"
  route         = "/user/{proxy+}"
}

module "mcce22_smart_office_userimages_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_users.function_arn
  function_name = module.mcce22_smart_office_users.function_name
  http_method   = "ANY"
  route         = "/userimage/{proxy+}"
}

module "mcce22_smart_office_workspaces_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_workspaces.function_arn
  function_name = module.mcce22_smart_office_workspaces.function_name
  http_method   = "ANY"
  route         = "/workspace/{proxy+}"
}

module "mcce22_smart_office_workspacedata_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_workspaces.function_arn
  function_name = module.mcce22_smart_office_workspaces.function_name
  http_method   = "ANY"
  route         = "/workspacedata/{proxy+}"
}

module "mcce22_smart_office_workspaceconfiguration_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_workspaces.function_arn
  function_name = module.mcce22_smart_office_workspaces.function_name
  http_method   = "ANY"
  route         = "/workspaceconfiguration/{proxy+}"
}

module "mcce22_smart_office_deviceactivator_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_device_activator.function_arn
  function_name = module.mcce22_smart_office_device_activator.function_name
  http_method   = "GET"
  route         = "/deviceactivator/{proxy+}"
}

module "mcce22_smart_office_bookings_proxy_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_bookings.function_arn
  function_name = module.mcce22_smart_office_bookings.function_name
  http_method   = "ANY"
  route         = "/booking/{proxy+}"
}

module "mcce22_smart_office_notifications_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_notifications.function_arn
  function_name = module.mcce22_smart_office_notifications.function_name
  http_method   = "POST"
  route         = "/notify/{proxy+}"
}

module "mcce22_smart_office_dataingress_api" {
  source        = "./modules/api-gateway-lambda-integration"
  api_id        = module.mcce22_smart_office_apigateway.api_id
  api_arn       = module.mcce22_smart_office_apigateway.api_arn
  function_arn  = module.mcce22_smart_office_dataingress.function_arn
  function_name = module.mcce22_smart_office_dataingress.function_name
  http_method   = "POST"
  route         = "/dataingress"
}