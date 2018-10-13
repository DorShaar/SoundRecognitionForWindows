using System;
using System.Collections.Generic;

namespace SoundRecognition
{
     class BasicSectionData : IRecordSectionData
    {
        private List<IRecordSliceData> m_recordSlicesList;
        //should use this to calculate intervals
        //will add intervals between the last recognition in a slice and 
        //the first recognition in following slice
        private List<double> m_recognitions; 

        private int m_indexInRecord;
        private Double m_startTime;
        private Boolean m_isTrendUp;
        private Boolean m_isTrendDown;
        private Boolean m_isPeak;//currently not used
        private Double m_totalTime;
        private Double m_totalTargetSoundRecognitions;
        private Double? m_currentMinInterval;
        private Double? m_currentMaxInterval;
        private Double m_avgInterval;   //TODO: should be easier to use intervals array and calculate when required
        private Double m_avgRecognitionsInSecond;
        private Double m_avgRecognitionsInSlice;
        private Double m_lastRecognitionTime;

        public BasicSectionData(int indexInRecord, List<IRecordSliceData> slices)
        {
            m_indexInRecord = indexInRecord;
            if (slices.Count == 0)
            {
                throw new Exception("Can't build section with empty slices list.");
            }
            m_recordSlicesList = slices;
            m_isPeak = false;
            m_isTrendDown = false;
            m_isTrendUp = false;
            m_totalTargetSoundRecognitions = 0;
            m_totalTime = 0;
            m_startTime = slices[0].getTimeInRecord();
            m_recognitions = new List<double>();
            init(slices);
            
        }

        private void init(List<IRecordSliceData> slices)
        {
            Double intervalsSum=0;
            //if (slices[0].getAmountOfTargetSoundRecognitions() > 0)
            //{
            if (slices[0].getMinInterval() != null && slices[0].getMaxInterval() != null)
            {
                m_currentMinInterval = (double)slices[0].getMinInterval();
                m_currentMaxInterval = (double)slices[0].getMaxInterval();
            }
            //}
            else
            {
                m_currentMaxInterval = null;
                m_currentMinInterval = null;
            }
            //TODO: use recognitions list to calculate intervals
            foreach (IRecordSliceData slice in slices)
            {
                if (slice.getMinInterval()!=null && slice.getMinInterval() < m_currentMinInterval)
                {
                    m_currentMinInterval = slice.getMinInterval();
                }
                if (slice.getMaxInterval() > m_currentMaxInterval)
                {
                    m_currentMaxInterval = slice.getMaxInterval();
                }
                m_totalTargetSoundRecognitions += slice.getAmountOfTargetSoundRecognitions();
                m_totalTime += slice.getDuration();
                if (slice.getAvgInterval() != null)
                {
                    intervalsSum += (double)slice.getAvgInterval();
                }
                if (slice.getLastRecognitionTime() !=null) //TODO: if slice contains no recognitions, return -1
                {
                    m_lastRecognitionTime = (double)slice.getLastRecognitionTime();
                }
                if (slice.getAmountOfTargetSoundRecognitions() > 0)
                {
                    //foreach(double recognition in slice.getRecognitionsList())
                    //{
                    //    m_recognitions.Add((double)(recognition));
                    //}
                    m_recognitions.AddRange(slice.getRecognitionsList()); //TODO: use this to calculate intervals
                }
            }

            m_avgRecognitionsInSlice = m_totalTargetSoundRecognitions / slices.Count;
            m_avgRecognitionsInSecond = m_totalTargetSoundRecognitions / m_totalTime;
            
        }

        public int getAmountOfSlices()
        {
            return m_recordSlicesList.Count;
        }
        public double getAvgIntervals()
        {
            return m_avgInterval;
        }

        public double getAvgRecognitionsInSecond()
        {
            return m_avgRecognitionsInSecond;
        }

        public double getAvgRecognitionsInSlice()
        {
            return m_avgRecognitionsInSlice;
        }

        public int getIndexInRecord()
        {
            return m_indexInRecord;
        }

        public double getLastRecognitionTime()
        {
            return m_lastRecognitionTime;
        }

        //TODO: use recognitions list to calculate intervals
        public double getMaxInterval()
        {
            double result = -1; //TODO: consts
            if (m_currentMaxInterval != null)
            {
                result = (double)m_currentMaxInterval;
            }
            return result;
        }
        //TODO: use recognitions list to calculate intervals
        public double getMinInterval()
        {
            double result = -1; //TODO: consts
            if (m_currentMinInterval != null)
            {
                result = (double)m_currentMinInterval;
            }
            return result;
        }

        public List<IRecordSliceData> getRecordSlicesList()
        {
            return m_recordSlicesList;
        }

        public double getStartTimeInRecord()
        {
            return m_startTime;
        }

        public double getTotalDuration()
        {
            return m_totalTime;
        }

        public double getTotalRecognitions()
        {
            return m_totalTargetSoundRecognitions;
        }

        public bool isTrendDown()
        {
            return m_isTrendDown;
        }

        public bool isTrendUp()
        {
            return m_isTrendUp;
        }

        public void setAsPeak()
        {
            m_isPeak = true;
        }
    }
}
