﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour {

    public int fuelAmount;
    public int ironAmount;
    public int shipOxygenAmount;                                                        // Not the same than in player inventory !!! Player inventory amount = part of this one

    public int scannerState;                                                                //0 - 1  0 = marche pas  1 = marche bien
    public int fuelTankState;                                                               //0 - 1
    public int oxygenTankState;                                                             //0 - 1

    private int ressourceRepareScanner = 10;
    private int ressourceRepareFuelTank = 10;
    private int ressourceRepareOxygenTank = 10;

    string OxyRepare_name  = "event:/OxyRepare";
    string FuelRepare_name = "event:/FuelRepare";
    string ScanRepare_name = "event:/ScanRepare";
    FMOD.Studio.EventInstance OxyRepare;
    FMOD.Studio.EventInstance FuelRepare;
    FMOD.Studio.EventInstance ScanRepare;

    public void ReadFile()
    {

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/player.save"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save").Close();
            string[] baseQuantity = new string[3];
            baseQuantity[0] = "0;0;0";//fuel, iron , playeroxygen
            baseQuantity[1] = "0;0;0;0";//tiles (not useful for this script)
            baseQuantity[2] = "1000;1;1;1";//shipoxygen, scanner, fuel tank and oxygen tank State
            System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", baseQuantity);
            fuelAmount = 4;
            ironAmount = 10;
            shipOxygenAmount = 1000;
            scannerState = 1;
            fuelTankState = 1;
            oxygenTankState = 1;
        }
        else
        {
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");

            string[] tmp = lines[0].Split(';');
            fuelAmount = int.Parse(tmp[0]);
            ironAmount = int.Parse(tmp[1]);

            string[] tmp2 = lines[2].Split(';');
            shipOxygenAmount = int.Parse(tmp2[0]);
            scannerState = int.Parse(tmp2[1]);
            fuelTankState = int.Parse(tmp2[2]);
            oxygenTankState = int.Parse(tmp2[3]);
        }
    }

    void Start () {
        ReadFile();
        shipOxygenAmount = 1000;    //

        OxyRepare = FMODUnity.RuntimeManager.CreateInstance(OxyRepare_name);
        FuelRepare = FMODUnity.RuntimeManager.CreateInstance(FuelRepare_name);
        ScanRepare = FMODUnity.RuntimeManager.CreateInstance(ScanRepare_name);
    }

    public void computeChangesToFile()
    {
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");
        string[] tmp = lines[0].Split(';');
        lines[0] = fuelAmount + ";" + ironAmount + ";" + tmp[2];
        lines[2] = shipOxygenAmount + ";" + scannerState + ";" + fuelTankState + ";" + oxygenTankState;
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", lines);
    }

    public int GetScannerState()
    {
        return scannerState;
    }

    public int GetFuelTankState()
    {
        return fuelTankState;
    }

    public int GetOxygenTankState()
    {
        return oxygenTankState;
    }

    public void SetScannerState(int etat)
    {
        scannerState = etat;
    }

    public void SetFuelTankState(int etat)
    {
        fuelTankState = etat;
    }

    public void SetOxygenTankState(int etat)
    {
        oxygenTankState = etat;
    }

    public void repareScanner()
    {   //ScanRepare.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        if( GetScannerState() == 0 && ironAmount - ressourceRepareScanner >= 0 )
        {
            ironAmount -= ressourceRepareScanner;
            SetScannerState(1);
            ScanRepare.start();
        }
    }

    public void repareFuelTank()
    {   //FuelRepare.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        if (GetFuelTankState() == 0 && ironAmount - ressourceRepareFuelTank >= 0)
        {
            ironAmount -= ressourceRepareFuelTank;
            SetFuelTankState(1);
            FuelRepare.start();
        }
    }

    public void repareOxygenTank()
    {   //OxyRepare.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        if (GetOxygenTankState() == 0 && ironAmount - ressourceRepareOxygenTank >= 0)
        {
            ironAmount -= ressourceRepareOxygenTank;
            SetOxygenTankState(1);
            OxyRepare.start();
        }
    }
}