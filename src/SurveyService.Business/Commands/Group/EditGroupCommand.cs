    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    DbGroup dbGroup = await _groupRepository.GetAsync(new GetGroupFilter { GroupId = GroupId });

    if (!await _accessValidator.IsAdminAsync(senderId) && senderId != dbGroup.CreatedBy)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.Forbidden,
        new List<string> { "You cannot edit this group." });
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
    }

    if (request.Operations.Any(op => op.path.EndsWith(nameof(EditGroupRequest.IsActive), StringComparison.OrdinalIgnoreCase))
      && !request.Operations
        .Where(op => op.path.EndsWith(nameof(EditGroupRequest.IsActive), StringComparison.OrdinalIgnoreCase))
        .Select(op =>
        {
          bool.TryParse(op.value.ToString(), out bool value);
          return value;
        })
        .First())
    {
      await _questionRepository.DisactivateAsync(dbGroup.Questions);
    }

    return new OperationResultResponse<bool>(
      body: await _groupRepository.EditAsync(_mapper.Map(request), dbGroup));
  }
}
