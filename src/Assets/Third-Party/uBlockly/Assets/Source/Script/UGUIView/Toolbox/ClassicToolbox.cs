﻿/****************************************************************************

Copyright 2016 sophieml1989@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UBlockly.UGUI
{
    public class ClassicToolbox : BaseToolbox
    {
        [SerializeField] protected GameObject m_MenuItemPrefab;
        [SerializeField] protected RectTransform m_MenuListContent;
        [SerializeField] protected GameObject m_BlockScrollList;
        [SerializeField] protected GameObject m_BlockContentPrefab;
        [SerializeField] protected GameObject m_BinArea;
        
        protected override void Build()
        {
            BuildMenu();
        }
        
        /// <summary>
        /// Build the left menu list, child class should implement this for custom build
        /// </summary>
        protected virtual void BuildMenu()
        {
            foreach (var category in mConfig.BlockCategoryList)
            {
                GameObject menuItem = GameObject.Instantiate(m_MenuItemPrefab, m_MenuListContent, false);
                menuItem.name = category.CategoryName;
                menuItem.GetComponentInChildren<Text>().text = I18n.Get(category.CategoryName);
                Image[] images = menuItem.GetComponentsInChildren<Image>();
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].color = category.Color;
                }
                menuItem.SetActive(true);
                
                Toggle toggle = menuItem.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((selected) =>
                {
                    if (selected)
                        ShowBlockCategory(menuItem.name);
                    else 
                        HideBlockCategory();
                });
                mMenuList[category.CategoryName] = toggle;
            }
        }
        
        public void ShowBlockCategory(string categoryName)
        {
            if (string.Equals(categoryName, mActiveCategory))
                return;
         
            if (!m_BlockScrollList.activeInHierarchy)
                m_BlockScrollList.SetActive(true);
            
            if (!string.IsNullOrEmpty(mActiveCategory))
                mRootList[mActiveCategory].SetActive(false);

            mActiveCategory = categoryName;
            
            GameObject contentObj;
            RectTransform contentTrans;
            if (mRootList.TryGetValue(categoryName, out contentObj))
            {
                contentObj.SetActive(true);
                contentTrans = contentObj.transform as RectTransform;
            }
            else
            {
                contentObj = GameObject.Instantiate(m_BlockContentPrefab, m_BlockContentPrefab.transform.parent);
                contentObj.name = "Content_" + categoryName;
                contentObj.SetActive(true);
                mRootList[categoryName] = contentObj;

                contentTrans = contentObj.GetComponent<RectTransform>();
                
                //build new blocks
                if (categoryName.Equals(Define.VARIABLE_CATEGORY_NAME))
                    BuildVariableBlocks();
                else if (categoryName.Equals(Define.PROCEDURE_CATEGORY_NAME))
                    BuildProcedureBlocks();
                else
                    BuildBlockViewsForActiveCategory();
            }

            //resize the background
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentTrans);
            m_BlockScrollList.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(contentTrans) * m_BlockContentPrefab.transform.localScale.x);
            
            m_BlockScrollList.GetComponent<ScrollRect>().content = contentTrans;
        }

        public void HideBlockCategory()
        {
            if (string.IsNullOrEmpty(mActiveCategory))
                return;

            mRootList[mActiveCategory].SetActive(false);
            mMenuList[mActiveCategory].isOn = false;
            m_BlockScrollList.SetActive(false);
            mActiveCategory = null;
        }

        /// <summary>
        /// Build block views for the active category, child class should implement this for custom build
        /// </summary>
        protected virtual void BuildBlockViewsForActiveCategory()
        {
            Transform contentTrans = mRootList[mActiveCategory].transform;
            var blockTypes = mConfig.GetBlockCategory(mActiveCategory).BlockList;

            List<GameObject> robots = new List<GameObject>();
            for (int i = 0; i < PlayerPrefs.GetInt("Robot count"); i++) {
                robots.Add(VaultManager.vaultInstance.getObject("Robot " + (i + 1)));
            }

            foreach (string blockType in blockTypes) {
                bool hasSensor = false;
                switch (blockType) {
                    case "sensors_touch_contact":
                        foreach(GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("contact")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_touch_not_contact":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("contact")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_ir_detect_white":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("infrared")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_ir_detect_black":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("infrared")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_color_detect_colors":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("color")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_us_distance":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("ultrasound")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_gyroscope_still":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("gyroscope")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "sensors_gyroscope_direction":
                        foreach (GameObject robot in robots) {
                            if (robot.GetComponent<RobotManager>().HasSensor("gyroscope")) {
                                hasSensor = true;
                            }
                        }
                        break;
                    case "robot_1":
                        if (VaultManager.vaultInstance.getObject("Robot 1") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_2":
                        if (VaultManager.vaultInstance.getObject("Robot 2") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_3":
                        if (VaultManager.vaultInstance.getObject("Robot 3") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_4":
                        if (VaultManager.vaultInstance.getObject("Robot 4") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_5":
                        if (VaultManager.vaultInstance.getObject("Robot 5") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_6":
                        if (VaultManager.vaultInstance.getObject("Robot 6") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_7":
                        if (VaultManager.vaultInstance.getObject("Robot 7") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_8":
                        if (VaultManager.vaultInstance.getObject("Robot 8") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_9":
                        if (VaultManager.vaultInstance.getObject("Robot 9") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    case "robot_10":
                        if (VaultManager.vaultInstance.getObject("Robot 10") != null) {
                            NewBlockView(blockType, contentTrans);
                        }
                        break;
                    //case "possible_new_sensor_block":
                    //  foreach (GameObject robot in robots) {
                    //    if (robot.GetComponent<RobotManager>().HasSensor("sensor to find")) {
                    //      hasSensor = true;
                    //    }
                    //  }
                    //break;
                    default:
                        NewBlockView(blockType, contentTrans);
                        break;
                    }
                if (hasSensor) {
                    NewBlockView(blockType, contentTrans);
                }
            }
        }

        protected override void OnPickBlockView()
        {
            HideBlockCategory();
        }

        public override bool CheckBin(BlockView blockView)
        {
            if (blockView.InToolbox) return false;
            
            RectTransform toggleTrans = m_BinArea.transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(toggleTrans, UnityEngine.Input.mousePosition, BlocklyUI.UICanvas.worldCamera))
            {
                m_BinArea.gameObject.SetActive(true);
                return true;
            }
            m_BinArea.gameObject.SetActive(false);
            return false;
        }

        public override void FinishCheckBin(BlockView blockView)
        {
            if (CheckBin(blockView))
                blockView.Dispose();
            m_BinArea.gameObject.SetActive(false);
        }
    }
}