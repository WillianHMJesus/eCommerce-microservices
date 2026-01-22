Feature: Password recovery

In order to regain access to my account
As a user
I want to reset my password using a recovery token

@SendUserToken
Scenario: Send password recovery token
	Given I am authenticated in with the "manager profile"
	And I have a registered user
	When I request a password recovery token
	Then The token must be sent

@SendUserToken
Scenario: Send password recovery token without being authenticated
	Given I am not authenticated
	And I have a registered user
	When I request a password recovery token
	Then the token must not be sent because the user is not authenticated

@SendUserToken
Scenario: Send password recovery token to unregistered user
	Given I am authenticated in with the "manager profile"
	And I do not have a registered user
	When I request a password recovery token
	Then It should return a success message even if no token is sent

@SendUserToken
Scenario: Send password recovery token with default email address
	Given I am authenticated in with the "manager profile"
	And I have a registered user
	When I request a password recovery token with default email address
	Then the token must not be sent
		| message                                   |
		| The email address cannot be null or empty |

@SendUserToken
Scenario: Send password recovery token with null email address
	Given I am authenticated in with the "manager profile"
	And I have a registered user
	When I request a password recovery token with null email address
	Then the token must not be sent
		| message                                   |
		| The email address cannot be null or empty |

@ValidateUserToken
Scenario: Validate password recovery token
	Given I am authenticated in with the "manager profile"
	And I have a password recovery token that has been sent
	When I request a password recovery token validation
	Then The token must be validated

@ValidateUserToken
Scenario: Validate password recovery token without being authenticated
	Given I am not authenticated
	And I have a password recovery token that has been sent
	When I request a password recovery token validation
	Then The token must not be validated because the user is not authenticated

@ValidateUserToken
Scenario: Validate password recovery token without the token having been sent
	Given I am authenticated in with the "manager profile"
	And I do not have a password recovery token that has been sent
	When I request a password recovery token validation
	Then The token must not be validated
		| message                  |
		| The user token not found |

@ValidateUserToken
Scenario: Validate password recovery token with expired token
	Given I am authenticated in with the "manager profile"
	And I have a password recovery token that has been sent
	When I request a password recovery token validation with the token has expired
	Then The token must not be validated
		| message                   |
		| The user token is expired |

@ValidateUserToken
Scenario: Validate password recovery token with invalid token
	Given I am not authenticated
	And I have a password recovery token that has been sent
	When I request a password recovery token validation
	Then The token must not be validated because the user is not authenticated

#Scenario: Reset password using valid token
#	Given I have a valid password recovery token
#	When I reset my password 
#	Then the response status code should be 200

