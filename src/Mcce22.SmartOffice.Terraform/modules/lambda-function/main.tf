data "archive_file" "lambda_archive" {
  type        = "zip"
  source_dir  = var.publish_dir
  output_path = "${path.module}/../../${var.zip_file}"
}

resource "aws_s3_object" "lambda_bundle" {
  bucket = var.lambda_bucket_id
  key    = var.zip_file
  source = data.archive_file.lambda_archive.output_path

  etag = filemd5(data.archive_file.lambda_archive.output_path)
}

resource "aws_lambda_function" "function" {
  function_name    = var.function_name
  s3_bucket        = var.lambda_bucket_id
  s3_key           = aws_s3_object.lambda_bundle.key
  runtime          = "dotnet6"
  handler          = var.lambda_handler
  source_code_hash = data.archive_file.lambda_archive.output_base64sha256
  role             = var.lambda_execution_role
  timeout          = 30
  memory_size      = 512
}

resource "aws_cloudwatch_log_group" "aggregator" {
  name = "/aws/lambda/${aws_lambda_function.function.function_name}"

  retention_in_days = 30
}
