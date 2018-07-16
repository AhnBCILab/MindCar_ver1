import numpy
import math

def stimInputCheck(self, index):
                result = False
                # <Stimulation Check>
                # Stimulation input : self.input[2] ~ self.input[7]
                # And this case, "Ind = self.input[2], Com = self.input[3], Cop = self.input[4]".
                for chunkIndex in range( len(self.input[index]) ):  #Ind case 
                        chunk = self.input[index].pop()             #Ind case 
                        if(type(chunk) == OVStimulationSet):
                                for stimIdx in range(len(chunk)):
                                        stim=chunk.pop();
                                        print('Received stim: ', stim.identifier, 'stamped at', stim.date, 's')
                                        # If stim.identifier's value is a one, then that case game is on!
                                        if(stim.identifier == 1):
                                                #IndState = True
                                                #onceCalculate_Ind = True
                                                result = True
                return result


def protocolSetting(protocol, standardDev2, syncMergeMean2):
    for j in range(len(standardDev2)):
        if j == 0:
            if syncMergeMean2 < standardDev2[j] :
                protocol = protocol + 1  # protocol += 1
                break
            elif standardDev2[j] <= syncMergeMean2 < standardDev2[j+1]:
                protocol = protocol + 2  # protocol += 2
                break
        elif j == (len(standardDev2) - 1):
            if standardDev2[j] <= syncMergeMean2  :
                protocol = protocol + len(standardDev2) + 1  # protocol += 11
                break
        else:
            if standardDev2[j] <= syncMergeMean2 < standardDev2[j+1]:
                protocol = protocol + 2 + j  # protocol += 3 ~ 10
                break
    return protocol            

	

class MyOVBox(OVBox):
	def __init__(self):
		OVBox.__init__(self)
		self.signalHeader = None

	def initialize(self):
                print('Python signal processing module... OK!')
                # For controling the conditions or states.
		global IndState
		global ComState
		global CopState
		global IndSignalProcessing
		global ComSignalProcessing
		global CopSignalProcessing

                # For calculating the data once for each cases.
                global onceCalculate_Ind                
                global onceCalculate_Com                
                global onceCalculate_Cop                
                global meanArr1
                global meanArr2
                global avg    

                # For dividing each standard deviations.
                global standardDev1
                global standardDev2

                # For processing the fixed window size (= 64) and for synchronization between two players
                global previousMean1    
                global previousMean2     
                global syncPreviousMean1
                global syncPreviousMean2
                global syncMergeMean1
                global syncMergeMean2


                # Initialize.
                IndState = False
                ComState = False
                CopState = False
                IndSignalProcessing = False
                ComSignalProcessing = False
                CopSignalProcessing = False
                onceCalculate_Ind = False   
                onceCalculate_Com = False 
                onceCalculate_Cop = False
                meanArr1=[]
                meanArr2=[]
                standardDev1 = [0,1,2,3,4,5,6,7,8,9]  # For ten guage value resolution.
                standardDev2 = [0,1,2,3,4,5,6,7,8,9]  # For ten guage value resolution.
                syncPreviousMean1 = 0
                syncPreviousMean2 = 0
                syncMergeMean1 = 0
                syncMergeMean2 = 0



	def process(self):
                # For controling the conditions or states.
		global IndState
		global ComState
		global CopState
		global IndSignalProcessing
		global ComSignalProcessing
		global CopSignalProcessing

                # For calculating the data once for each cases.
                global onceCalculate_Ind                
                global onceCalculate_Com                
                global onceCalculate_Cop                
                global meanArr1
                global meanArr2
                global avg    

                # For dividing each standard deviations.
                global standardDev1
                global standardDev2

                # For processing the fixed window size (= 64) and for synchronization between two players
                global previousMean1    
                global previousMean2    
                global syncPreviousMean1
                global syncPreviousMean2
                global syncMergeMean1
                global syncMergeMean2


                # <Stimulation checking>   Reset = 7
                ResetCase = stimInputCheck(self, 7)
                if(ResetCase):
                        print('Initialize and Reset!')
                        IndState = False
                        ComState = False
                        CopState = False
                        EndCase  = False
                        IndSignalProcessing = False
                        ComSignalProcessing = False
                        CopSignalProcessing = False



                # When 30s comes in(get stim), compute the ST, interval
                if(onceCalculate_Ind):

                        print(meanArr1)
                        print('length of meanArr1: ', len(meanArr1))
                        
                        ### signal processing
                        
                        N=len(meanArr1)          # Numer of Mean
                        sumM=0                  
                        sumV=0

                        # Average
                        for i in range(0,N):
                                sumM+=meanArr1[i]
                        avg = sumM/N

                        # Variance                                      
                        for i in range(0,N):
                                sumV+=math.pow(meanArr1[i]-avg,2)
                        myVar = sumV/(N-1)

                        # Deviation
                        myDev = math.sqrt(myVar)

                        print("Average: ", avg, "/  Variance : ", myVar, "/  Deviation: ", myDev)
                        
                        # Compute the intervals for received data. There are ten intervals ( 0.2 ~ 2.0 )
                        weight = 0.2
                        for i in range(len(standardDev1)):
                            standardDev1[i] = avg + (myDev*weight)
                            weight = weight + 0.2
                            print(i+1, " Interval : ", standardDev1[i])
                            
                        onceCalculate_Ind = False

                # When 30s comes in(get stim), compute the ST, interval
                if(onceCalculate_Com):

                        print(meanArr1)
                        print('length of meanArr1: ', len(meanArr1), ' / length of meanArr2: ', len(meanArr2))
                        print(meanArr2)
                        
                        ### signal processing - Player 1                     
                        N=len(meanArr1)          # Numer of Mean
                        sumM=0                  
                        sumV=0

                        # Average
                        for i in range(0,N):
                                sumM+=meanArr1[i]
                        avg = sumM/N

                        # Variance                                      
                        for i in range(0,N):
                                sumV+=math.pow(meanArr1[i]-avg,2)
                        myVar = sumV/(N-1)

                        # Deviation
                        myDev = math.sqrt(myVar)

                        print("Player 1 - Average: ", avg, "/  Variance : ", myVar, "/  Deviation: ", myDev)

                        # Compute the intervals for received data. There are ten intervals ( 0.2 ~ 2.0 ) - Player 1
                        weight = 0.2
                        for i in range(len(standardDev1)):
                            standardDev1[i] = avg + (myDev*weight)
                            weight = weight + 0.2
                            print(i+1, " Interval - Player1 : ", standardDev1[i])

                        ### signal processing - Player 2                       
                        N=len(meanArr2)          # Numer of Mean
                        sumM=0                  
                        sumV=0

                        # Average
                        for i in range(0,N):
                                sumM+=meanArr2[i]
                        avg = sumM/N

                        # Variance                                      
                        for i in range(0,N):
                                sumV+=math.pow(meanArr2[i]-avg,2)
                        myVar = sumV/(N-1)

                        # Deviation
                        myDev = math.sqrt(myVar)

                        print("Player 2 - Average: ", avg, "/  Variance : ", myVar, "/  Deviation: ", myDev)

                        # Compute the intervals for received data. There are ten intervals ( 0.2 ~ 2.0 ) - Player 2
                        weight = 0.2
                        for i in range(len(standardDev2)):
                            standardDev2[i] = avg + (myDev*weight)
                            weight = weight + 0.2
                            print(i+1, " Interval - Player2 : ", standardDev2[i])
                            
                        onceCalculate_Com = False

                        
                if(IndState):
                        ### It is a Individual starting state which is a training step in game.

                        # <Stimulation checking>   Ind = 2,  End = 5
                        IndCase = stimInputCheck(self, 2)
                        EndCase = stimInputCheck(self, 5)
                        if(IndCase):
                                print('Individual training is completed!')
                                IndState = False
                                onceCalculate_Ind = True
                                IndSignalProcessing = True          

                        if(EndCase):
                                IndState = False
                                ComState = False
                                CopState = False
                                EndCase  = False
                                IndSignalProcessing = False
                                ComSignalProcessing = False
                                CopSignalProcessing = False
                                
                        # <Signal processing>
                        # Signal input :  player1 = self.input[0] 
                        #                 signal output = self.output[0]  / stimulation output = self.output[1]
                        for chunkIndex in range( len(self.input[0]) ):
                                if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[0].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[0].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                        mean = (numpy.mean(numpyBuffer))
                                        #print("Individual training state - mean : ")
        				#print(mean)

                                        # Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        mergeMean = (previousMean1 + mean)/2
                                        previousMean1 = mean

                                        # Fix the protocol to 0 before geting stim(30s or 35s)
                                        protocol = 0

                                        meanArr1.append(mergeMean)
                                                                                                      
                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
                                elif(type(self.input[0][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[0].pop())
                                        
                elif(ComState):
                        ### It is a Competition starting state which is a training step in game.

                        # <Stimulation checking>   Com = 3,  End = 5
                        ComCase = stimInputCheck(self, 3)
                        EndCase = stimInputCheck(self, 5)
                        if(ComCase):
                                print('Competition training is completed!')
                                syncPreviousMean1 = 0
                                syncPreviousMean2 = 0
                                ComState = False
                                onceCalculate_Com = True
                                ComSignalProcessing = True          

                        if(EndCase):
                                IndState = False
                                ComState = False
                                CopState = False
                                EndCase  = False
                                IndSignalProcessing = False
                                ComSignalProcessing = False
                                CopSignalProcessing = False
                                
                        ### <Signal processing> ###
                        # Signal input :  player1 = self.input[0]
                        for chunkIndex in range( len(self.input[0]) ):
                                if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[0].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[0].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                        mean1 = (numpy.mean(numpyBuffer))

                                        # Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        mergeMean1 = (previousMean1 + mean1)/2                                        
                                        if(syncPreviousMean1 == 0):
                                                syncPreviousMean1 = mean1 

                                        # Fix the protocol to 0 before geting stim(30s or 35s)
                                        protocol = 0

                                        meanArr1.append(mergeMean1)
                                                                                                      
                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
                                elif(type(self.input[0][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[0].pop())

                        # Signal input :  player2 = self.input[1]
                        for chunkIndex in range( len(self.input[1]) ):
                                if(type(self.input[1][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[1].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[1][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[1].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                        mean2 = (numpy.mean(numpyBuffer))

                                        # Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        mergeMean2 = (previousMean2 + mean2)/2
                                        if(syncPreviousMean2 == 0):
                                                syncPreviousMean2 = mean2

                                        # Fix the protocol to 0 before geting stim(30s or 35s)
                                        protocol = 0

                                        meanArr2.append(mergeMean2)
                                                                                                      
                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
                                elif(type(self.input[1][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[1].pop())

                        # For synchronization
                        if(syncPreviousMean1 != 0 and syncPreviousMean2 != 0):                            
                                print('syncPreviousMean1 : ', syncPreviousMean1,'syncPreviousMean2 : ', syncPreviousMean2)
                                previousMean1 = syncPreviousMean1
                                previousMean2 = syncPreviousMean2
                                syncPreviousMean1 = 0
                                syncPreviousMean2 = 0
    
                elif(CopState):
                        IndState = False
                elif(IndSignalProcessing):
                        ### It is a Individual gaming state.
                        
                        # <Stimulation checking>   End = 5
                        EndCase = stimInputCheck(self, 5)
                        if(EndCase):
                                print('Individual game is finished!')
                                IndState = False
                                ComState = False
                                CopState = False
                                EndCase  = False
                                IndSignalProcessing = False
                                ComSignalProcessing = False
                                CopSignalProcessing = False
                                meanArr1=[]
                                            
                        ### <Signal processing> ###
                        for chunkIndex in range( len(self.input[0]) ):
                                if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[0].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], 
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[0].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                # mean is received mean of the signal after 30s. 
        				mean = (numpy.mean(numpyBuffer))        				
        				
        				# Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        mergeMean = (previousMean1 + mean)/2
                                        previousMean1 = mean

                                        #print("Individual gaming state mean : ", mergeMean)
                                        
                                # Determine the protocols ( 1 ~ 10 )     
                                        for i in range(len(standardDev1)-1):
                                            if standardDev1[i] <= mergeMean < standardDev1[i+1]:
                                                protocol = i+1 # Then it will be start from the number 1.
                                        if standardDev1[0] > mergeMean:
                                                protocol = 0  # default speed
                                        if standardDev1[len(standardDev1)-1] <= mergeMean:
                                                protocol = len(standardDev1) # => 10

                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
				
                                elif(type(self.input[0][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[0].pop())

                       

                        
                elif(ComSignalProcessing):
                        ### It is a Competition gaming state.
                        
                        # <Stimulation checking>   End = 5
                        EndCase = stimInputCheck(self, 5)
                        if(EndCase):
                                print('Competition game is finished!')
                                IndState = False
                                ComState = False
                                CopState = False
                                EndCase  = False
                                IndSignalProcessing = False
                                ComSignalProcessing = False
                                CopSignalProcessing = False
                                
                        ### <Signal processing> ###
                        # Signal input :  player1 = self.input[0]
                        for chunkIndex in range( len(self.input[0]) ):
                                if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[0].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], 
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[0].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                # mean is received mean of the signal after 30s. 
        				mean1 = (numpy.mean(numpyBuffer))        				
        				
        				# Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        #mergeMean1 = (previousMean1 + mean1)/2
                                        if(syncMergeMean1 == 0):                                                
                                                syncMergeMean1 = (previousMean1 + mean1)/2
                                                previousMean1 = mean1
                                        
                                elif(type(self.input[1][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[1].pop())

                        # Signal input :  player2 = self.input[1]
                        for chunkIndex in range( len(self.input[1]) ):
                                if(type(self.input[1][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[1].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[1][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[1].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                
                                        
                                # mean is received mean of the signal after 30s. 
        				mean2 = (numpy.mean(numpyBuffer))        				
        				
        				# Merge the previous mean and current mean for making the fixed window size(32+32 = 64)                                        
                                        #syncMergeMean2 = (previousMean2 + mean2)/2
                                        if(syncMergeMean2 == 0):                                                
                                                syncMergeMean2 = (previousMean2 + mean2)/2
                                                previousMean2 = mean2

                                        # For synchronization
                                        if(syncMergeMean1 != 0 and syncMergeMean2 != 0):
                                                print('syncMergeMean1 : ', syncMergeMean1,'syncMergeMean2 : ', syncMergeMean2)

                                                # Determine the protocols ( 1 ~ 121 )  11*11 => Since each 10 resolutions and 1 default
                                                protocol = 0

                                                for i in range(len(standardDev1)):                                                    
                                                    if i == 0:
                                                        if syncMergeMean1 < standardDev1[i] : 
                                                            protocol = 0  # protocol = 1 ~ 11
                                                            protocol = protocolSetting(protocol, standardDev2, syncMergeMean2)
                                                            break
                                                        elif standardDev1[i] <= syncMergeMean1 < standardDev1[i+1]: 
                                                            protocol = len(standardDev2) + 1  # protocol = 12 ~ 22
                                                            protocol = protocolSetting(protocol, standardDev2, syncMergeMean2)                                                          
                                                            break

                                                    elif i == (len(standardDev1) - 1): 
                                                        if standardDev1[i] <= syncMergeMean1  :
                                                            protocol = (len(standardDev1) + 1) * len(standardDev1) # protocol = 111 ~ 121
                                                            protocol = protocolSetting(protocol, standardDev2, syncMergeMean2)                                                          
                                                            break
                                                    else:
                                                        protocol = (len(standardDev1) + 1) * (i+1)  # protocol = 23 ~ 110
                                                        if standardDev1[i] <= syncMergeMean1 < standardDev1[i+1]:
                                                            protocol = protocolSetting(protocol, standardDev2, syncMergeMean2)                                                          
                                                            break
                                                        
                                                syncMergeMean1 = 0
                                                syncMergeMean2 = 0

                                                print('protocol : ', protocol) ######################
                                                                              
                                                protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                                chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                                self.output[0].append(chunk)      
                                elif(type(self.input[1][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[1].pop())
                elif(CopSignalProcessing):
                        IndState = False
                else :
                ### It is a default state that just sends a mean signal.

                        # <Stimulation checking>   Ind = 2, Com = 3, Cop = 4, End = 5
                        IndCase = stimInputCheck(self, 2)
                        ComCase = stimInputCheck(self, 3)                        
                        CopCase = stimInputCheck(self, 4)
                        if(IndCase):
                                print('Individual Game and Training Start!')                                
                                IndState = True
                        if(ComCase):
                                print('Competition Game and Training Start!')
                                meanArr1=[]
                                meanArr2=[]
                                syncPreviousMean1 = 0
                                syncPreviousMean2 = 0
                                ComState = True
                        if(CopCase):
                                print('Cooperation Game and Training Start!')
                                CopState = True
                                
                        ### <Signal processing> ###
                        # Signal output = self.output[0]  / stimulation output = self.output[1]
                        
                        # Signal input :  player1 = self.input[0]  
                        for chunkIndex in range( len(self.input[0]) ):
                                if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[0].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[0].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                        mean = (numpy.mean(numpyBuffer))
                                        #print("Default state - mean : ")
        				#print(mean)

                                        # Store current mean to a 'previous mean' for fixed window size.
                                        previousMean1 = mean
                                        if(syncPreviousMean1 == 0):
                                                syncPreviousMean1 = mean 
                                        
                                        

                                        # Fix the protocol to 0 before geting stim(30s or 35s)
                                        protocol = 0                                        
                                                                                                      
                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
                                elif(type(self.input[0][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[0].pop())
                                        
                        # Signal input :  player2 = self.input[1]
                        for chunkIndex in range( len(self.input[1]) ):
                                if(type(self.input[1][chunkIndex]) == OVSignalHeader):
                                        self.signalHeader = self.input[1].pop()
                                        outputHeader = OVSignalHeader(
                                                self.signalHeader.startTime,
                                                self.signalHeader.endTime,
                                                [1,1], # It is modified for new a structure.
                                                ['Mean']+self.signalHeader.dimensionSizes[1]*[''],
                                                self.signalHeader.samplingRate)
                                        self.output[0].append(outputHeader)
                                elif(type(self.input[1][chunkIndex]) == OVSignalBuffer):
                                        chunk = self.input[1].pop()
                                        numpyBuffer = numpy.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                                        numpyBuffer = numpyBuffer.mean(axis=0)
                                        
                                        mean = (numpy.mean(numpyBuffer))
                                        #print("Default state - mean : ")
        				#print(mean)

                                        # Store current mean to a 'previous mean' for fixed window size.
                                        if(syncPreviousMean2 == 0):
                                                syncPreviousMean2 = mean 

                                        # Fix the protocol to 0 before geting stim(30s or 35s)
                                        protocol = 0                                        
                                                                                                      
                                        protocolBuffer = numpy.array(protocol).reshape(tuple([1]))
                                        chunk = OVSignalBuffer(chunk.startTime, chunk.endTime, protocolBuffer.tolist())
                                        self.output[0].append(chunk)
                                elif(type(self.input[1][chunkIndex]) == OVSignalEnd):
                                        self.output[0].append(self.input[1].pop())
                                        
                        # For synchronization
                        if(syncPreviousMean1 != 0 and syncPreviousMean2 != 0):                            
                                #print('syncPreviousMean1 : ', syncPreviousMean1,'syncPreviousMean2 : ', syncPreviousMean2)
                                previousMean1 = syncPreviousMean1
                                previousMean2 = syncPreviousMean2
                                syncPreviousMean1 = 0
                                syncPreviousMean2 = 0
                                






box = MyOVBox()
