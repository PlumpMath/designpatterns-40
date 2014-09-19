//
//  Tokenizer.cpp
//  tokenizer
//
//  Created by Alex Nagelkerke on 19-09-14.
//  Copyright (c) 2014 Alex Nagelkerke. All rights reserved.
//

#include <stdio.h>
#include <iostream>
#include <algorithm>
#include <functional>
#include "Tokenizer.h"

Tokenizer::Tokenizer(std::string fileLocation, std::list<TokenDefinition> definitions, std::list<TokenPartner> partners)
{
	// set defaults
	lineNumber = 0;
	linePosition = 1;
	level = 1;
	lineRemaining = "";

	tokenDefinitions = definitions;
	tokenPartners = partners;

	file.open(fileLocation);

	nextLine();
}

void Tokenizer::Tokenize()
{
	tokenList = new std::list<Token>();

	while (lineRemaining.length() != 0)
	{
		lineRemaining = trim(lineRemaining);
		bool match = false;

		std::list<TokenDefinition>::iterator tokenDef;
		for (tokenDef = tokenDefinitions.begin(); tokenDef != tokenDefinitions.end(); ++tokenDef)
		{
			TokenDefinition tokenD = *tokenDef;
			int matched = tokenD.matcher->Match(lineRemaining);

			if (matched > 0)
			{
				match = true;

				std::string tokenValue = lineRemaining.substr(0, matched);

				if (tokenValue == "(" || tokenValue == "{" || tokenValue == "[")
					level++;

				Token *partner = nullptr;
				if (tokenD.tokenType == TokenType::CloseCurlyBracket ||
					tokenD.tokenType == TokenType::CloseBracket ||
					tokenD.tokenType == TokenType::Keyword)
					partner = findPartner(tokenValue, level);

				Token *token = new Token(lineNumber, linePosition, level, tokenValue, tokenD.tokenType, partner);

				tokenList->push_back(*token);

				if (tokenValue == ")" || tokenValue == "}" || tokenValue == "]")
					level--;

				linePosition += matched;
				lineRemaining = lineRemaining.substr(matched);

				if (lineRemaining.length() == 0)
					nextLine();

				break;
			}
		}
	}
}

Token* Tokenizer::findPartner(std::string &tokenStr, int level)
{
	Token *token = nullptr;
	std::list<TokenPartner>::iterator tokenPartner;
	for (tokenPartner = tokenPartners.begin(); tokenPartner != tokenPartners.end(); ++tokenPartner)
	{
		TokenPartner tokenP = *tokenPartner;
		if (tokenP.token == tokenStr)
		{
			std::list<Token>::reverse_iterator tokenIt;
			for (tokenIt = tokenList->rbegin(); tokenIt != tokenList->rend(); ++tokenIt)
			{
				token = &(*tokenIt);
				if (token->Value == tokenP.partner && token->Level == level)
				{
					return token;
				}
			}
			return token;
		}
	}

	return token;
}

void Tokenizer::nextLine()
{
	while (std::getline(file, lineRemaining))
	{
		++lineNumber;
		linePosition = 1;

		if (lineRemaining.length() > 0)
			break;
	}
}

std::list<Token>* Tokenizer::GetTokenList()
{
	return tokenList;
}

// From Stackoverflow http://stackoverflow.com/a/217605
// trim from start
std::string Tokenizer::ltrim(std::string &s) {
	s.erase(s.begin(), std::find_if(s.begin(), s.end(), std::not1(std::ptr_fun<int, int>(std::isspace))));
	return s;
}

// trim from end
std::string Tokenizer::rtrim(std::string &s) {
	s.erase(std::find_if(s.rbegin(), s.rend(), std::not1(std::ptr_fun<int, int>(std::isspace))).base(), s.end());
	return s;
}

// trim from both ends
std::string Tokenizer::trim(std::string &s) {
	std::string temp = rtrim(s);
	return ltrim(temp);
}