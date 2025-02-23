SELECT AspNetUsers.Id,
AspNetUsers.Email,
NormalizedName
  FROM [dbo].[AspNetUsers]
  INNER JOIN [dbo].[AspNetUserRoles]
  ON AspNetUsers.id = [AspNetUserRoles].UserId
    INNER JOIN [dbo].[AspNetRoles]
	ON [AspNetRoles].id = [AspNetUserRoles].RoleId
  WHERE Email = 'admin@test.com'