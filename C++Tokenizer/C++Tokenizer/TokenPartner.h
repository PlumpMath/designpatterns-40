//
//  TokenPartner.h
//  tokenizer
//
//  Created by Alex Nagelkerke on 19-09-14.
//  Copyright (c) 2014 Alex Nagelkerke. All rights reserved.
//

#include <string>

class TokenPartner
{
public:
	std::string token;
	std::string partner;
	TokenPartner(std::string tokenStr, std::string partnerStr);
};