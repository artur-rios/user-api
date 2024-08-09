﻿using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Core.Dto
{
    public class OperationResultDto<T>(T? data, string[] messages, Results result = Results.Success)
    {
        public T? Data { get; } = data;
        public string[] Messages { get; } = messages;
        public Results Result { get; } = result;
    }
}