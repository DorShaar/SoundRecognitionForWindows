using System;
using System.Collections.Generic;
using System.Linq;

namespace KNN
{
    internal class BasicRecordSliceData : IRecordSliceData
    {
        private List<Double> m_recognitonsTimes;
        private double m_duration;
        private int m_indexInRecord;
        private double m_startingTime;

        public BasicRecordSliceData(int index, double startingTime,double duration, List<Double> recognationTimes)
        {
            m_indexInRecord = index;
            m_startingTime = startingTime;
            m_duration = duration;
            m_recognitonsTimes = new List<double>(recognationTimes);
        }

        public BasicRecordSliceData(int index, double startingTime, double duration)
        {
            m_indexInRecord = index;
            m_startingTime = startingTime;
            m_duration = duration;
            m_recognitonsTimes = new List<double>();
        }

        public void addRecognition(Double recognitionTime)
        {
            m_recognitonsTimes.Add(recognitionTime);
        }
        public int getAmountOfTargetSoundRecognitions()
        {
            return m_recognitonsTimes.Count;
        }

        public double? getAvgInterval()
        {
            double? result = null;
            double intervalsSum = 0;
            if (m_recognitonsTimes.Count > 1)
            {
                for (int i = 0; i < m_recognitonsTimes.Count - 1; i++)
                {
                    intervalsSum += m_recognitonsTimes[i + 1] - m_recognitonsTimes[i];
                }
                result = intervalsSum / (m_recognitonsTimes.Count - 1);
            }
            else if (m_recognitonsTimes.Count == 1)
            {
                result = m_duration - m_recognitonsTimes[0];
            }

            return result;
        }

        public double getDuration()
        {
            return m_duration;
        }

        public int getIndexInRecord()
        {
            return m_indexInRecord;
        }

        public double? getLastRecognitionTime()
        {
            double? result = null;
            if (m_recognitonsTimes.Count > 0)
            {
                result = m_recognitonsTimes.Last(); 
            }
            return result;
        }

        public double? getMaxInterval()
        {
            double? result = null;
            double currentInterval, max = -1;
            for(int i= 0;i < m_recognitonsTimes.Count - 1; i++)
            {
                currentInterval = m_recognitonsTimes[i + 1] - m_recognitonsTimes[i];
                if (currentInterval > max)
                {
                    max = currentInterval;
                }
            }
            if (max >= 0)
            {
                result = max;
            }
            return result;
        }

        public double? getMinInterval()
        {
            double? min = null;
            double currentInterval;
            for (int i = 0; i < m_recognitonsTimes.Count - 1; i++)
            {
                currentInterval = m_recognitonsTimes[i + 1] - m_recognitonsTimes[i];
                if (min==null || currentInterval < min)
                {
                    min = currentInterval;
                }
            }
            
            return min;
        }

        public double getTimeInRecord()
        {
            return m_startingTime;
        }

        public List<double> getRecognitionsList()
        {
            return m_recognitonsTimes;
        }
    }
}
