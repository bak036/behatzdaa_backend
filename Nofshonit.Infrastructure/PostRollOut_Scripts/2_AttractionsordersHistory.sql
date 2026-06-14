USE [Histadrut]
GO

/****** Object:  View [dbo].[AttractionsOrdersHistory]    Script Date: 16/04/2019 12:22:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO













CREATE VIEW [dbo].[AttractionsOrdersHistory]
AS
SELECT dbo.[GetCategoryName](cv.CategoryNumber) as CategoryName, cv.CategoryNumber, pv.BusinessSubTypeID, pv.BusinessId,
		ao.LastImplementationDate, pv.EndDate, wst.TTransactionDateTime, pv.FullBarCode as FullBarCode,
		pv.VarNameApp as shortNameVar, wst.TTransactionOrder, wst.CustomerPrice, wst.Coins, wst.TTransactionquantity,
		ao.OrderId, o.ExternalGuid, pv.IsSendToFriend, o.FriendName, o.FriendMobile,
		pv.LoadingAmount, ao.MemberOrderDateEXE AS OrderDateExe, o.IsSentToFriend, o.CreditCard16Digits, o.CreditCardExpirey,
		ao.CardNumber, ao.MemberID, ao.OriginalMemberId, ao.MemberOrderAsmchta AS OrderAsmchta, pv.BusinessName, bs.BusinessSubTypeName,
		CASE WHEN pv.RedimTypeId > 0 THEN pv.RedimTypeId ELSE CASE WHEN pc.IsRedimTypeOtherMessage = 1 THEN 999 ELSE pc.RedimTypeId END END AS RedimTypeId,
		CASE WHEN pv.RedimTypeId > 0 THEN rtNormal.RedimName ELSE CASE WHEN pc.IsRedimTypeOtherMessage = 1 THEN pc.RedimTypeOtherMessageText ELSE rtFather.RedimName END END AS RedimTypeName,
		b.AutoImplementaionAfterReport, s.DaysBeforeShowToAllowCancel, sc.DaysRangeToCancel, o.Slink, ao.OrderDate,
		pc.LinkToTheatre
FROM ATRACTIONSOrders ao
JOIN WebServiceTransaction wst ON ao.MemberOrderAsmchta = wst.TTransactionOrder
JOIN Orders o ON o.OrderId = ao.OrderId
JOIN ProductsVars pv ON pv.FullBarCode = wst.TTransactionProductID
JOIN CategoryVariants cv ON pv.FullBarCode = cv.Barcode

--JOIN UserPortalCategories upc ON upc.CategoryNumber = cv.CategoryNumber
JOIN DTS_Online..PortalCategories pc ON cv.CategoryNumber = pc.CategoryNumber
JOIN DTS_Online..business b ON b.BuisnessID = pv.BusinessId
JOIN DTS_Online..BusinessSubType bs ON b.BusinessSubTypeID = bs.BusinessSubTypeId

JOIN Histadrut..BusinessSubTypeSpecificationCurrent sc ON sc.BusinessSubTypeId = b.BusinessSubTypeID
LEFT JOIN TicketsHub..Setup s ON s.OrganizationId = 102
LEFT JOIN DTS_Online..RedimTypes rtNormal ON rtNormal.RedimTypeId = pv.RedimTypeId
LEFT JOIN DTS_Online..RedimTypes rtFather ON rtFather.RedimTypeId = pc.RedimTypeId
where b.StoreType not in(1,4,7)

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[9] 2[37] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ao"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 257
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wst"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pv"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 399
               Right = 292
            End
            DisplayFlags = 280
            TopColumn = 105
         End
         Begin Table = "cv"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 497
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "o"
            Begin Extent = 
               Top = 402
               Left = 257
               Bottom = 531
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "upc"
            Begin Extent = 
               Top = 534
               Left = 38
               Bottom = 663
               Right = 308
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pc"
            Begin Extent = 
               Top = 666
               Left = 38
               Bottom = 795
               Right = 308
            End
            DisplayFlags = 280
            TopColumn = 0
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AttractionsOrdersHistory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   End
         Begin Table = "bs"
            Begin Extent = 
               Top = 798
               Left = 38
               Bottom = 927
               Right = 274
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 930
               Left = 38
               Bottom = 1059
               Right = 295
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sc"
            Begin Extent = 
               Top = 1062
               Left = 38
               Bottom = 1191
               Right = 291
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 1194
               Left = 38
               Bottom = 1323
               Right = 295
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rtNormal"
            Begin Extent = 
               Top = 6
               Left = 295
               Bottom = 101
               Right = 465
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rtFather"
            Begin Extent = 
               Top = 102
               Left = 295
               Bottom = 197
               Right = 465
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AttractionsOrdersHistory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AttractionsOrdersHistory'
GO


