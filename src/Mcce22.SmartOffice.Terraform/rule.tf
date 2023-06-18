resource "aws_iot_topic_rule" "mcce22_dataingress_rule" {
  name        = "mcce22dataingressrule"
  enabled     = true
  sql         = "SELECT * FROM 'mcce22-smart-factory/dataingress'"
  sql_version = "2016-03-23"
  lambda {
    function_arn = module.mcce22_smart_office_dataingress.function_arn
  }
}

resource "aws_lambda_permission" "mcce22_dataingress_permission" {
  function_name = module.mcce22_smart_office_dataingress.function_name
  action        = "lambda:InvokeFunction"
  principal     = "iot.amazonaws.com"
  source_arn    = aws_iot_topic_rule.mcce22_dataingress_rule.arn
}
