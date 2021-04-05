/*
 * timeseries.h
 *
 *  Created on: 26 срхїз 2020
 *      Author: Eli
 */

#ifndef TIMESERIES_H_
#define TIMESERIES_H_
#include <iostream>
#include <string.h>
#include <fstream>
#include<map>
#include <vector>
#include <sstream> 
#include <string.h>
//#include <bits/stdc++.h>

#include <algorithm>
using namespace std;

class TimeSeries{


	map<string,vector<float>> ts;
	vector<string> atts;
	size_t dataRowSize;
public:


	TimeSeries(const char* CSVfileName){
		ifstream in(CSVfileName);
		string head;
		in>>head;
		string val;
		stringstream hss(head);
		int i = 0;
		//create the first line vectors
		while(getline(hss,val,',')){
			vector<float> v;
			v.push_back(stof(val));
		     ts[to_string(i)]=v;
		     atts.push_back(to_string(i));
		     i++;
		}

		while(!in.eof()){
			string line;
			in>>line;
			string val;
			stringstream lss(line);
			int i=0; //starting from the second line (index 1)
			while(getline(lss,val,',')){
				ts[atts[i]].push_back(stof(val));
			     i++;
			}
		}
		in.close();

		dataRowSize = ts[atts[0]].size();

	}

	const vector<float>& getAttributeData(string name)const{
		return ts.at(name);
	}

	const vector<string>& gettAttributes()const{
		return atts;
	}

	size_t getRowSize()const{
		return dataRowSize;
	}

	~TimeSeries(){

	}
};



#endif /* TIMESERIES_H_ */
