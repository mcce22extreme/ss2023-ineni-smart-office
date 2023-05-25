resource "random_string" "random_s3_identifier" {
  length  = 4
  special = false
  lower   = true
  upper   = false
  numeric = false
}

resource "aws_s3_bucket" "mcce22_smart_office_lambda" {
  bucket        = "mcce-smart-office-lambda-${random_string.random_s3_identifier.result}"
  force_destroy = true
}

resource "aws_s3_bucket" "mcce22_smart_office_userimage" {
  bucket        = "mcce-smart-office-userimage-${random_string.random_s3_identifier.result}"
  force_destroy = true
}

resource "aws_s3_bucket_public_access_block" "mcce22_smart_office_userimage" {
  bucket                  = aws_s3_bucket.mcce22_smart_office_userimage.id
  block_public_acls       = false
  ignore_public_acls      = false
  block_public_policy     = false
  restrict_public_buckets = false
}

data "aws_iam_policy_document" "mcce22_smart_office_userimage_public_access" {
  statement {
    principals {
      type        = "AWS"
      identifiers = ["*"]
    }
    effect = "Allow"
    actions = [
      "s3:GetObject",
      "s3:ListBucket",
    ]
    resources = [
      aws_s3_bucket.mcce22_smart_office_userimage.arn,
      "${aws_s3_bucket.mcce22_smart_office_userimage.arn}/*",
    ]
  }
}

resource "aws_s3_bucket_policy" "mcce22_smart_office_userimage" {
  bucket = aws_s3_bucket.mcce22_smart_office_userimage.id
  policy = data.aws_iam_policy_document.mcce22_smart_office_userimage_public_access.json
}

# resource "aws_s3_bucket_policy" "mcce22_smart_office_userimage" {
#   bucket     = aws_s3_bucket.mcce22_smart_office_userimage.id
#   depends_on = [aws_s3_bucket.mcce22_smart_office_userimage]
#   policy     = <<EOF
#   {
#     "Version": "2012-10-17",
#     "Statement": [
#       {
#         "Effect": "Allow",
#         "Principal": "*",
#         "Action": [ "s3:GetObject" ],
#         "Resource": [
#           "${aws_s3_bucket.mcce22_smart_office_userimage.arn}"
#         ]
#       }
#     ]
#   }
#   EOF
# }
