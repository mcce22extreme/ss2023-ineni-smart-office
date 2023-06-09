﻿using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.Workspaces.Entities
{
    [DynamoDBTable("mcce22-smart-office-workspace-configurations")]
    public class WorkspaceConfiguration
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public long DeskHeight { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string WorkspaceId { get; set; }

        [DynamoDBProperty]
        public string WorkspaceUser { get; set; }

        [DynamoDBProperty]
        public string FirstName { get; set; }

        [DynamoDBProperty]
        public string LastName { get; set; }

        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string WorkspaceNumber { get; set; }

        [DynamoDBProperty]
        public string RoomNumber { get; set; }
    }
}
