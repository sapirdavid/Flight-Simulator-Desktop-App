#pragma once
#include <stdio.h>
#include <string>
#include <OleAuto.h>
#include "timeseries.h"
#include <string>
#include <utility>
#include "SimpleAnomalyDetector.h"


//class test {
	
//public:
	//test(int x);
	//int adding(int y);
	
//};

/*
extern "C" __declspec(dllexport) int add(test * a ,int y) {
	return a->adding(y);
}
*/
extern "C" __declspec(dllexport) void pHi() {
	printf("hi from c ");
}

extern "C" __declspec(dllexport) int pi() {
	return 5555;
}


extern "C" __declspec(dllexport) const  char *  pBi(char * s) {
	const char* b = "ffgrsgd";
	char a[6] = "tttt";
	return b;
}
//extern "C" __declspec(dllexport) const  void setNormal(char* s) {
	//normal = TimeSeries(s);
	//anomalyDetector.detect(normal);
//}
extern "C" __declspec(dllexport) int* detect(char* normalCsvPath, char* anomalyCsvPath) {	

	TimeSeries normal(normalCsvPath);

	SimpleAnomalyDetector anomalyDetector;
	anomalyDetector.learnNormal(normal);

	//first argument in the pair is the anomalie property and the second is the line
	vector<vector<pair<int, int>>> anomalies;
	for (int i = 0; i < normal.gettAttributes().size(); i++) {
		anomalies.push_back(vector<pair<int, int>>());
	}
	
	TimeSeries anomalyTs(anomalyCsvPath);
	vector<AnomalyReport> reports = anomalyDetector.detect(anomalyTs);
	//entrer tha anomlies to anomalies
	for (size_t i = 0; i < reports.size(); i++)
	{
		string columnsStr = reports[i].description;
		std::stringstream columns(columnsStr);
		std::string column;
		std::string secondColumn;
		std::getline(columns, column, '-');
		std::getline(columns, secondColumn, '-');
		anomalies[stoi(column)].push_back(std::pair<int,int>(stoi(secondColumn),reports[i].timeStep));
		anomalies[stoi(secondColumn)].push_back(std::pair<int, int>(stoi(column), reports[i].timeStep));
	}

	//end of property is -1 
	for (size_t i = 0; i < anomalyTs.gettAttributes().size(); i++)
	{
		anomalies[i].push_back(std::pair<int,int>(-1,-1));//end of section ends with -1
	}

	vector<int> anomalyAttributes;
	vector<int> anomalyLine;
	for (size_t i = 0; i < anomalies.size(); i++) {
		for (size_t j = 0; j < anomalies[i].size(); j++)
		{
			anomalyAttributes.push_back(anomalies[i][j].first);
			anomalyLine.push_back(anomalies[i][j].second); //push anomaly row

		}
	}
	int fullSize = anomalyAttributes.size() + anomalyLine.size();
	int firstsize = anomalyAttributes.size();
	int secondSize = anomalyLine.size();

	int* anomaliesAttributesAndLines = new int[fullSize + 1];
	anomaliesAttributesAndLines[0] = fullSize;
	
	int i = 1;
	for ( int k=0; k < firstsize; i++, k++) {
		anomaliesAttributesAndLines[i] = anomalyAttributes[k];
	}
	for (int k = 0; k < secondSize; i++, k++) {
		anomaliesAttributesAndLines[i] = anomalyLine[k];
	}

	return anomaliesAttributesAndLines;
}
extern "C" __declspec(dllexport) void del(int *n) {
	delete []n;
}