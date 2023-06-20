resource "aws_dynamodb_table" "mcce22_smart_office_users_table" {
  name           = "mcce22-smart-office-users"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "Id"
    type = "S"
  }
}

resource "aws_dynamodb_table" "mcce22_smart_office_userimages_table" {
  name           = "mcce22-smart-office-userimages"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "Id"
    type = "S"
  }
  attribute {
    name = "UserId"
    type = "S"
  }
  global_secondary_index {
    name            = "UserId-index"
    hash_key        = "UserId"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
}

resource "aws_dynamodb_table" "mcce22_smart_office_workspaces_table" {
  name           = "mcce22-smart-office-workspaces"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "Id"
    type = "S"
  }
  attribute {
    name = "WorkspaceNumber"
    type = "S"
  }
  global_secondary_index {
    name            = "WorkspaceNumber-index"
    hash_key        = "WorkspaceNumber"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
}

resource "aws_dynamodb_table" "mcce22_smart_office_workspaces_configuration_table" {
  name           = "mcce22-smart-office-workspace-configurations"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "Id"
    type = "S"
  }
  attribute {
    name = "WorkspaceId"
    type = "S"
  }
  attribute {
    name = "UserId"
    type = "S"
  }
  attribute {
    name = "WorkspaceUser"
    type = "S"
  }
  global_secondary_index {
    name            = "WorkspaceId-index"
    hash_key        = "WorkspaceId"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
  global_secondary_index {
    name            = "UserId-index"
    hash_key        = "UserId"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
  global_secondary_index {
    name            = "WorkspaceUser-index"
    hash_key        = "WorkspaceUser"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
}

resource "aws_dynamodb_table" "mcce22_smart_office_workspacedata_table" {
  name           = "mcce22-smart-office-workspace-data"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "Id"
    type = "S"
  }
  attribute {
    name = "Timestamp"
    type = "S"
  }
  attribute {
    name = "WorkspaceId"
    type = "S"
  }
  global_secondary_index {
    name            = "Timestamp-index"
    hash_key        = "Timestamp"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
  global_secondary_index {
    name            = "WorkspaceId-index"
    hash_key        = "WorkspaceId"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
}

resource "aws_dynamodb_table" "mcce22_smart_office_bookings_table" {
  name           = "mcce22-smart-office-bookings"
  billing_mode   = "PROVISIONED"
  read_capacity  = 5
  write_capacity = 5
  hash_key       = "Id"
  attribute {
    name = "StartDate"
    type = "S"
  }
  attribute {
    name = "Id"
    type = "S"
  }
  attribute {
    name = "ActivationCode"
    type = "S"
  }
  global_secondary_index {
    name            = "StartDate-index"
    hash_key        = "StartDate"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
  global_secondary_index {
    name            = "ActivationCode-index"
    hash_key        = "ActivationCode"
    projection_type = "ALL"
    read_capacity   = 5
    write_capacity  = 5
  }
}
