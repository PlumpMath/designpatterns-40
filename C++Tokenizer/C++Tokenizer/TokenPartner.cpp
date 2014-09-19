//
//  TokenPartner.cpp
//  tokenizer
//
//  Created by Alex Nagelkerke on 19-09-14.
//  Copyright (c) 2014 Alex Nagelkerke. All rights reserved.
//

#include <stdio.h>
#include "TokenPartner.h"

TokenPartner::TokenPartner(std::string tokenStr, std::string partnerStr)
{
	token = tokenStr;
	partner = partnerStr;
}